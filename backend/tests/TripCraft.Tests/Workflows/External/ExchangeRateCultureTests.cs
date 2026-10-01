using System.Globalization;
using System.Net;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using TripCraft.Infrastructure.External;

namespace TripCraft.Tests.Workflows.External;

public class ExchangeRateCultureTests
{
    [Theory]
    [InlineData("en-US")]
    [InlineData("fr-FR")]
    [InlineData("de-DE")]
    public async Task Decimal_fallback_configuration_is_independent_of_server_culture(string culture)
    {
        var previous = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(culture);
            using var cache = new MemoryCache(new MemoryCacheOptions());
            using var http = StubHandler.Status(HttpStatusCode.ServiceUnavailable).Client();
            var service = new ExchangeRateService(http, cache,
                StubHandler.Config(("FX_FALLBACK_LKR_PER_USD", "305.75")),
                NullLogger<ExchangeRateService>.Instance);

            var rate = await service.GetUsdToLkrAsync(CancellationToken.None);

            rate.Rate.Should().Be(305.75m);
            rate.Stale.Should().BeTrue();
            rate.AsOf.Should().Be(DateTime.UnixEpoch);
        }
        finally
        {
            CultureInfo.CurrentCulture = previous;
        }
    }
}
