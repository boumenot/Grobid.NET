using FluentAssertions;
using Xunit;

using Grobid.NET;


namespace Grobid.Test
{
    public class ModelScorerTest
    {
        [Fact]
        public void ModelScorerTruePositive()
        {
            var testSubject = new ModelScorer();
            testSubject.Score("label", "label");

            testSubject.Stat.LabelStats["label"].Expected.Should().Be(1);
            testSubject.Stat.LabelStats["label"].Observed.Should().Be(1);
            testSubject.Stat.LabelStats["label"].FalseNegative.Should().Be(0);
            testSubject.Stat.LabelStats["label"].FalsePositive.Should().Be(0);
        }

        [Fact]
        public void ModelScorerFalsePositive()
        {
            var testSubject = new ModelScorer();
            testSubject.Score("expected", "obtained");
            testSubject.Score("expected", "obtained");

            testSubject.Stat.LabelStats["expected"].Expected.Should().Be(1);
            testSubject.Stat.LabelStats["expected"].Observed.Should().Be(0);
            testSubject.Stat.LabelStats["expected"].FalseNegative.Should().Be(2);
            testSubject.Stat.LabelStats["expected"].FalsePositive.Should().Be(0);

            testSubject.Stat.LabelStats["obtained"].Expected.Should().Be(1);
            testSubject.Stat.LabelStats["obtained"].Observed.Should().Be(0);
            testSubject.Stat.LabelStats["obtained"].FalseNegative.Should().Be(0);
            testSubject.Stat.LabelStats["obtained"].FalsePositive.Should().Be(2);
        }

        [Fact]
        public void ModelScorerFalsePositiveLabelIsNew()
        {
            var testSubject = new ModelScorer();
            testSubject.Score("expected", "obtained");

            testSubject.Stat.LabelStats["expected"].Expected.Should().Be(0);
            testSubject.Stat.LabelStats["expected"].Observed.Should().Be(0);
            testSubject.Stat.LabelStats["expected"].FalseNegative.Should().Be(1);
            testSubject.Stat.LabelStats["expected"].FalsePositive.Should().Be(0);

            testSubject.Stat.LabelStats["obtained"].Expected.Should().Be(1);
            testSubject.Stat.LabelStats["obtained"].Observed.Should().Be(0);
            testSubject.Stat.LabelStats["obtained"].FalseNegative.Should().Be(0);
            testSubject.Stat.LabelStats["obtained"].FalsePositive.Should().Be(1);
        }

    }
}
