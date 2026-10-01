using FluentAssertions;
using TripCraft.Application.Quotations;
using TripCraft.Application.Quotations.Dtos;

namespace TripCraft.Tests.Quotations;

public class QuotationListValidatorTests
{
    [Fact]
    public void Optional_filters_can_be_omitted_and_zero_minimum_is_allowed()
    {
        var validator = new QuotationListQueryValidator();
        validator.Validate(new QuotationListQuery()).IsValid.Should().BeTrue();
        validator.Validate(new QuotationListQuery { MinTotalUsd = 0, Status = QuotationStatus.Approved })
            .IsValid.Should().BeTrue();
    }

    [Fact]
    public void Invalid_status_and_negative_minimum_are_rejected()
    {
        var result = new QuotationListQueryValidator().Validate(new QuotationListQuery
        {
            Status = (QuotationStatus)999, MinTotalUsd = -0.01m
        });
        result.Errors.Should().Contain(e => e.PropertyName == "Status");
        result.Errors.Should().Contain(e => e.PropertyName == "MinTotalUsd");
    }

    [Fact]
    public void Reversed_dates_are_rejected_but_open_ranges_are_allowed()
    {
        var validator = new QuotationListQueryValidator();
        var start = new DateOnly(2026, 10, 1);
        validator.Validate(new QuotationListQuery { From = start, To = start.AddDays(-1) })
            .Errors.Should().Contain(e => e.PropertyName == "To");
        validator.Validate(new QuotationListQuery { From = start }).IsValid.Should().BeTrue();
        validator.Validate(new QuotationListQuery { To = start }).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0, 20)]
    [InlineData(1, 0)]
    [InlineData(1, 101)]
    public void Invalid_pagination_is_rejected(int page, int pageSize)
    {
        new QuotationListQueryValidator().Validate(new QuotationListQuery { Page = page, PageSize = pageSize })
            .IsValid.Should().BeFalse();
    }

    [Fact]
    public void Unknown_sort_field_is_rejected()
    {
        new QuotationListQueryValidator().Validate(new QuotationListQuery { Sort = "unknownField" })
            .Errors.Should().Contain(e => e.PropertyName == "Sort");
    }
}
