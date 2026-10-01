using FluentAssertions;
using TripCraft.Application.Quotations;
using TripCraft.Application.Quotations.Dtos;
using TripCraft.Application.Quotations.Reports;

namespace TripCraft.Tests.Quotations;

public class QuotationEndDateTests
{
    [Theory]
    [InlineData(9999, 12, 31, false)]
    [InlineData(9999, 12, 30, true)]
    public void Quotation_end_date_must_allow_next_day(
        int year, int month, int day, bool expectedValid)
    {
        var result = new QuotationListQueryValidator().Validate(
            new QuotationListQuery { To = new DateOnly(year, month, day) });

        result.IsValid.Should().Be(expectedValid);
        if (!expectedValid)
            result.Errors.Should().Contain(e => e.PropertyName == "To");
    }

    [Theory]
    [InlineData(9999, 12, 31, false)]
    [InlineData(9999, 12, 30, true)]
    public void Report_end_date_must_allow_next_day(
        int year, int month, int day, bool expectedValid)
    {
        var end = new DateOnly(year, month, day);
        var result = new ReportRangeQueryValidator().Validate(
            new ReportRangeQuery { From = end, To = end });

        result.IsValid.Should().Be(expectedValid);
        if (!expectedValid)
            result.Errors.Should().Contain(e => e.PropertyName == "To");
    }

    [Fact]
    public void Quotation_end_date_can_be_omitted()
    {
        new QuotationListQueryValidator()
            .Validate(new QuotationListQuery())
            .IsValid.Should().BeTrue();
    }
}