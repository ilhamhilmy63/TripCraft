using FluentAssertions;
using TripCraft.Application.Quotations;

namespace TripCraft.Tests.Quotations;

public class DecisionCommentBoundaryTests
{
    [Theory]
    [InlineData(1, true)]
    [InlineData(999, true)]
    [InlineData(1000, true)]
    [InlineData(1001, false)]
    public void Both_decision_forms_enforce_the_same_comment_limit(int length, bool valid)
    {
        var comment = new string('x', length);
        var approval = new QuotationDecisionRequestValidator().Validate(new QuotationDecisionRequest(comment));
        var revision = new RequestRevisionRequestValidator().Validate(new RequestRevisionRequest(comment));
        approval.IsValid.Should().Be(valid);
        revision.IsValid.Should().Be(valid);
        if (!valid)
        {
            approval.Errors.Should().ContainSingle().Which.PropertyName.Should().Be("Comment");
            revision.Errors.Should().ContainSingle().Which.PropertyName.Should().Be("Comment");
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("\t\r\n")]
    public void Revision_requires_meaningful_feedback_even_when_approval_does_not(string? comment)
    {
        new QuotationDecisionRequestValidator().Validate(new QuotationDecisionRequest(comment)).IsValid.Should().BeTrue();
        new RequestRevisionRequestValidator().Validate(new RequestRevisionRequest(comment!)).Errors
            .Should().ContainSingle().Which.PropertyName.Should().Be("Comment");
    }
}
