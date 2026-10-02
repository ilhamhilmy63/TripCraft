using FluentAssertions;
using TripCraft.Application.Quotations;

namespace TripCraft.Tests.Quotations;

public class QuotationInputBoundaryTests
{
    [Theory]
    [InlineData(-1, 100)]
    [InlineData(1, -100)]
    [InlineData(-1, -100)]
    [InlineData(0, 100)]
    [InlineData(1, 0)]
    public void Nonpositive_items_do_not_change_a_valid_quote(decimal qty, decimal unit)
    {
        var quote = QuotationCalculator.Calculate([
            new PriceItem("guide", "Valid guide", 2, 100),
            new PriceItem("room", "Invalid room", qty, unit)], 10, 2);

        quote.Lines.Should().ContainSingle().Which.Description.Should().Be("Valid guide");
        quote.SubtotalLkr.Should().Be(200);
        quote.MarginLkr.Should().Be(20);
        quote.TotalLkr.Should().Be(220);
        quote.TotalUsd.Should().Be(110);
    }

    [Fact]
    public void Fractional_quantities_preserve_line_identity_and_input_order()
    {
        var quote = QuotationCalculator.Calculate([
            new PriceItem("vehicle", "Airport transfer", 1.25m, 10),
            new PriceItem("entry", "Museum", 2, 3)], 0, 1);

        quote.Lines.Should().Equal(
            new CalculatedLine("vehicle", "Airport transfer", 1.25m, 10, 12.5m),
            new CalculatedLine("entry", "Museum", 2, 3, 6));
        quote.TotalLkr.Should().Be(18.5m);
    }
}
