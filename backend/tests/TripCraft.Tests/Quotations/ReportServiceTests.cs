using FluentAssertions;
using Moq;
using TripCraft.Application.Quotations.Reports;

namespace TripCraft.Tests.Quotations;

public class ReportServiceTests
{
    [Fact]
    public async Task Utilisation_matches_holds_by_resource_type_and_id()
    {
        var start = new DateOnly(2026, 10, 1);
        var sharedId = Guid.NewGuid();
        var idleId = Guid.NewGuid();
        var unrelatedId = Guid.NewGuid();
        var query = new ReportRangeQuery { From = start, To = start.AddDays(9) };
        var queries = new Mock<IReportQueries>();
        IReadOnlyList<ResourceName> resources =
        [
            new("Guide", sharedId, "Guide with hold"),
            new("Vehicle", sharedId, "Vehicle with hold"),
            new("Guide", idleId, "Idle guide")
        ];
        IReadOnlyList<HoldSpan> holds =
        [
            new("Guide", sharedId, start.AddDays(-2), start.AddDays(1)),
            new("Vehicle", sharedId, start.AddDays(7), start.AddDays(12)),
            new("Guide", unrelatedId, start, start.AddDays(9))
        ];
        queries.Setup(q => q.GuidesAndVehiclesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(resources);
        queries.Setup(q => q.HoldsAsync(query.From, query.To, It.IsAny<CancellationToken>()))
            .ReturnsAsync(holds);

        var result = await new ReportService(queries.Object).UtilisationAsync(query, CancellationToken.None);

        result.Should().HaveCount(3);
        result.Should().ContainSingle(r => r.ResourceType == "Guide" && r.ResourceId == sharedId)
            .Which.Should().Be(new UtilisationDto("Guide", sharedId, "Guide with hold", 2, 10, 20m));
        result.Should().ContainSingle(r => r.ResourceType == "Vehicle" && r.ResourceId == sharedId)
            .Which.Should().Be(new UtilisationDto("Vehicle", sharedId, "Vehicle with hold", 3, 10, 30m));
        result.Should().ContainSingle(r => r.ResourceId == idleId)
            .Which.Should().Be(new UtilisationDto("Guide", idleId, "Idle guide", 0, 10, 0m));
    }
}
