using FluentAssertions;
using TripCraft.Application.Quotations;

namespace TripCraft.Tests.Quotations;

public class QuotationRoundingTests
{
    [Fact]
    public void Each_line_is_rounded_before_subtotal_and_usd_conversion()
    {
        var result = QuotationCalculator.Calculate(
            [new PriceItem("entry", "First", 1, 1.005m), new PriceItem("entry", "Second", 1, 1.005m)], 0, 2);

        result.Lines.Should().OnlyContain(l => l.AmountLkr == 1.01m);
        result.SubtotalLkr.Should().Be(2.02m);
        result.TotalLkr.Should().Be(2.02m);
        result.TotalUsd.Should().Be(1.01m);
    }

    [Fact]
    public void Margin_and_currency_midpoints_round_away_from_zero()
    {
        var result = QuotationCalculator.Calculate([new PriceItem("guide", "Guide", 1, 1)], 0.5m, 2);
        result.MarginLkr.Should().Be(0.01m);
        result.TotalLkr.Should().Be(1.01m);
        result.TotalUsd.Should().Be(0.51m);
    }

    [Fact]
    public void Empty_items_produce_zero_totals()
    {
        var result = QuotationCalculator.Calculate([], 15m, 300m);
        result.Lines.Should().BeEmpty();
        result.SubtotalLkr.Should().Be(0);
        result.MarginLkr.Should().Be(0);
        result.TotalLkr.Should().Be(0);
        result.TotalUsd.Should().Be(0);
    }

    [Fact]
    public void Negative_margin_is_rejected()
    {
        var calculate = () => QuotationCalculator.Calculate([new PriceItem("guide", "Guide", 1, 100)], -0.01m, 300m);
        calculate.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("marginPct");
    }
}
