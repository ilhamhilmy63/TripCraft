using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using TripCraft.Infrastructure.External;

namespace TripCraft.Tests.Workflows.External;

public class ExchangeRateRecoveryTests
{
    private const string Success = """{"result":"success","time_last_update_unix":1790812800,"rates":{"LKR":310}}""";

    private static ExchangeRateService Service(StubHandler handler, MemoryCache cache) =>
        new(handler.Client(), cache, StubHandler.Config(), NullLogger<ExchangeRateService>.Instance);

    [Fact]
    public async Task Caller_cancellation_propagates_without_caching_a_fallback()
    {
        using var cache = new MemoryCache(new MemoryCacheOptions());
        using var cancellation = new CancellationTokenSource();
        var handler = new StubHandler(_ =>
        {
            cancellation.Cancel();
            throw new OperationCanceledException(cancellation.Token);
        });

        var act = () => Service(handler, cache).GetUsdToLkrAsync(cancellation.Token);

        await act.Should().ThrowAsync<OperationCanceledException>();
        cache.Count.Should().Be(0);
    }

    [Fact]
    public async Task Provider_timeout_without_caller_cancellation_uses_a_stale_fallback()
    {
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var rate = await Service(StubHandler.Throws(new TaskCanceledException("Provider timed out")), cache)
            .GetUsdToLkrAsync(default);

        rate.Rate.Should().Be(300);
        rate.Stale.Should().BeTrue();
        rate.AsOf.Should().Be(DateTime.UnixEpoch);
    }

    [Fact]
    public async Task A_success_after_an_outage_replaces_fallback_with_a_cached_fresh_rate()
    {
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var failed = await Service(StubHandler.Throws(new HttpRequestException("Offline")), cache)
            .GetUsdToLkrAsync(default);
        var handler = StubHandler.Json(Success);
        var service = Service(handler, cache);
        var recovered = await service.GetUsdToLkrAsync(default);
        var cached = await service.GetUsdToLkrAsync(default);

        failed.Stale.Should().BeTrue();
        recovered.Rate.Should().Be(310);
        recovered.Stale.Should().BeFalse();
        cached.Should().Be(recovered);
        handler.Requests.Should().ContainSingle();
    }
}
