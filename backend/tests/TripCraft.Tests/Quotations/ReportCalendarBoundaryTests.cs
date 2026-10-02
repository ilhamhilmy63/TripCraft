using FluentAssertions;
using TripCraft.Application.Quotations.Reports;

namespace TripCraft.Tests.Quotations;

public class ReportCalendarBoundaryTests
{
    [Theory]
    [InlineData(2024, 2, 28, 2024, 3, 1, 3)]
    [InlineData(2025, 2, 28, 2025, 3, 1, 2)]
    [InlineData(2026, 12, 31, 2027, 1, 1, 2)]
    [InlineData(9999, 12, 31, 9999, 12, 31, 1)]
    public void Reporting_days_include_both_ends_across_calendar_boundaries(
        int y1, int m1, int d1, int y2, int m2, int d2, int expected)
    {
        ReportMath.DaysInRange(new(y1, m1, d1), new(y2, m2, d2)).Should().Be(expected);
    }

    [Theory]
    [InlineData(-2, 0, 1)]
    [InlineData(2, 4, 1)]
    [InlineData(-2, -1, 0)]
    [InlineData(3, 4, 0)]
    [InlineData(-2, 4, 3)]
    public void Holds_touching_either_report_boundary_count_that_day(int start, int end, int expected)
    {
        var from = new DateOnly(2024, 2, 28);
        ReportMath.ClippedDays(from.AddDays(start), from.AddDays(end), from, from.AddDays(2))
            .Should().Be(expected);
    }
}
