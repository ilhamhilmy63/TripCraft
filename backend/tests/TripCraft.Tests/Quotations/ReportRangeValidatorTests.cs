using FluentAssertions;
using TripCraft.Application.Quotations;
using TripCraft.Application.Quotations.Reports;

namespace TripCraft.Tests.Quotations;

public class ReportRangeValidatorTests
{
    [Fact]
    public void Missing_start_date_is_rejected()
    {
        var result = new ReportRangeQueryValidator().Validate(new ReportRangeQuery { To = new DateOnly(2026, 10, 1) });
        result.Errors.Should().Contain(e => e.PropertyName == "From");
    }

    [Fact]
    public void Reversed_range_is_rejected()
    {
        var result = new ReportRangeQueryValidator().Validate(new ReportRangeQuery
        {
            From = new DateOnly(2026, 10, 2), To = new DateOnly(2026, 10, 1)
        });
        result.Errors.Should().Contain(e => e.PropertyName == "To");
    }

    [Fact]
    public void Single_day_and_leap_year_ranges_are_accepted()
    {
        var validator = new ReportRangeQueryValidator();
        validator.Validate(new ReportRangeQuery { From = new(2024, 2, 29), To = new(2024, 2, 29) })
            .IsValid.Should().BeTrue();
        validator.Validate(new ReportRangeQuery { From = new(2024, 1, 1), To = new(2024, 12, 31) })
            .IsValid.Should().BeTrue();
    }

    [Fact]
    public void Multi_year_range_is_rejected()
    {
        new ReportRangeQueryValidator().Validate(new ReportRangeQuery
        {
            From = new(2024, 1, 1), To = new(2026, 1, 1)
        }).IsValid.Should().BeFalse();
    }
}
