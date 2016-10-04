using FluentAssertions;
using Xunit;

using Grobid.NET;

namespace Grobid.Test
{
    public class ModelStatTest
    {
        [Fact]
        public void LabelStatsAreProvisionedOnDemand()
        {
            var testSubject = new ModelStat();

            testSubject.Labels.Should().BeEmpty();
            testSubject.IncrExpected("label");
            testSubject.Labels.Should().HaveCount(1);
        }

        [Fact]
        public void LabelStatsAreIncremented()
        {
            var testSubject = new ModelStat();

            testSubject.IncrExpected("label");
            testSubject.LabelStats["label"].Expected.Should().Be(1);
            testSubject.LabelStats["label"].Observed.Should().Be(0);
            testSubject.LabelStats["label"].FalseNegative.Should().Be(0);
            testSubject.LabelStats["label"].FalsePositive.Should().Be(0);

            testSubject.IncrObserved("label");
            testSubject.IncrFalseNegative("label");
            testSubject.IncrFalsePositive("label");

            testSubject.LabelStats["label"].Expected.Should().Be(1);
            testSubject.LabelStats["label"].Observed.Should().Be(1);
            testSubject.LabelStats["label"].FalseNegative.Should().Be(1);
            testSubject.LabelStats["label"].FalsePositive.Should().Be(1);
        }
    }
}
