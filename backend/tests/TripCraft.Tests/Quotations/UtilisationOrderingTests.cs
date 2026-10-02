using FluentAssertions;
using Moq;
using TripCraft.Application.Quotations.Reports;

namespace TripCraft.Tests.Quotations;

public class UtilisationOrderingTests
{
    [Fact]
    public async Task Resources_sort_by_type_then_usage_then_name_and_keep_idle_rows()
    {
        var start = new DateOnly(2026, 10, 2);
        var ids = Enumerable.Range(0, 5).Select(_ => Guid.NewGuid()).ToArray();
        IReadOnlyList<ResourceName> resources = [
            new("Vehicle", ids[0], "Van"), new("Guide", ids[1], "Zara"),
            new("Guide", ids[2], "Idle"), new("Guide", ids[3], "Alex"),
            new("Guide", ids[4], "Busy")];
        IReadOnlyList<HoldSpan> holds = [
            new("Vehicle", ids[0], start, start),
            new("Guide", ids[1], start, start),
            new("Guide", ids[3], start, start),
            new("Guide", ids[4], start, start.AddDays(1))];
        var queries = new Mock<IReportQueries>();
        queries.Setup(q => q.GuidesAndVehiclesAsync(default)).ReturnsAsync(resources);
        queries.Setup(q => q.HoldsAsync(start, start.AddDays(1), default)).ReturnsAsync(holds);

        var rows = await new ReportService(queries.Object).UtilisationAsync(
            new() { From = start, To = start.AddDays(1) }, default);

        rows.Select(r => r.Name).Should().Equal("Busy", "Alex", "Zara", "Idle", "Van");
        rows.Select(r => r.UtilisationPct).Should().Equal(100m, 50m, 50m, 0m, 50m);
    }

    [Fact]
    public async Task Duplicate_full_range_holds_cannot_report_more_than_full_capacity()
    {
        var start = new DateOnly(2026, 10, 2);
        var id = Guid.NewGuid();
        var queries = new Mock<IReportQueries>();
        queries.Setup(q => q.GuidesAndVehiclesAsync(default))
            .ReturnsAsync(new ResourceName[] { new("Guide", id, "Guide") });
        queries.Setup(q => q.HoldsAsync(start, start, default))
            .ReturnsAsync(new HoldSpan[] { new("Guide", id, start, start), new("Guide", id, start, start) });

        var rows = await new ReportService(queries.Object).UtilisationAsync(new() { From = start, To = start }, default);

        rows.Should().ContainSingle().Which.Should().Be(new UtilisationDto("Guide", id, "Guide", 1, 1, 100m));
    }
}
