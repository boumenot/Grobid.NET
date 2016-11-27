using System.Linq;

using FluentAssertions;
using Xunit;

using Grobid.NET.Scoring;

namespace Grobid.Test.Scoring
{
    public class ModelScorerTest
    {
        [Fact]
        public void ModelScorerTruePositive()
        {
            var testSubject = new ModelScorer();
            testSubject.Eval("label", "label");

            testSubject.Stat.LabelStats["label"].Expected.Should().Be(1);
            testSubject.Stat.LabelStats["label"].Observed.Should().Be(1);
            testSubject.Stat.LabelStats["label"].FalseNegative.Should().Be(0);
            testSubject.Stat.LabelStats["label"].FalsePositive.Should().Be(0);
        }

        [Fact]
        public void ModelScorerFalsePositive()
        {
            var testSubject = new ModelScorer();
            testSubject.Eval("expected", "obtained");
            testSubject.Eval("expected", "obtained");

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
            testSubject.Eval("expected", "obtained");

            testSubject.Stat.LabelStats["expected"].Expected.Should().Be(0);
            testSubject.Stat.LabelStats["expected"].Observed.Should().Be(0);
            testSubject.Stat.LabelStats["expected"].FalseNegative.Should().Be(1);
            testSubject.Stat.LabelStats["expected"].FalsePositive.Should().Be(0);

            testSubject.Stat.LabelStats["obtained"].Expected.Should().Be(1);
            testSubject.Stat.LabelStats["obtained"].Observed.Should().Be(0);
            testSubject.Stat.LabelStats["obtained"].FalseNegative.Should().Be(0);
            testSubject.Stat.LabelStats["obtained"].FalsePositive.Should().Be(1);
        }

        [Fact]
        public void ModelScorerTokenLevelResults()
        {
            var scorer = new ModelScorer();
            scorer.Eval("one", "one");
            scorer.Eval("one", "one");
            scorer.Eval("one", "one");

            scorer.Eval("one", "two");
            scorer.Eval("one", "two");

            scorer.Eval("two", "two");

            var testSubjects = scorer.Scores().ToArray();
            var testSubject1 = testSubjects.Single(x => x.Label == "one");
            testSubject1.Accuracy.Should().Be(0.75);
            testSubject1.Precision.Should().Be(1.0);
            testSubject1.Recall.Should().Be(0.6);
            testSubject1.F0.Should().BeApproximately(0.74999, 0.00001);

            var testSubject2 = testSubjects.Single(x => x.Label == "two");
            testSubject2.Accuracy.Should().Be(0.75);
            testSubject2.Precision.Should().BeApproximately(0.33333, 0.00001);
            testSubject2.Recall.Should().Be(1.0);
            testSubject2.F0.Should().Be(0.5);
        }

        [Fact]
        public void ModelScorerCumulativeScores()
        {
            var scorer = new ModelScorer();
            scorer.Eval("one", "one");
            scorer.Eval("one", "one");
            scorer.Eval("one", "one");

            scorer.Eval("one", "two");
            scorer.Eval("one", "two");

            scorer.Eval("two", "two");

            var testSubject = CumulativeScore.Create(scorer.Scores().ToArray());

            testSubject.MicroLabelScore.Accuracy.Should().Be(0.5);
            testSubject.MicroLabelScore.Precision.Should().BeApproximately(0.66667, 0.00001);
            testSubject.MicroLabelScore.Recall.Should().BeApproximately(0.66667, 0.00001);
            testSubject.MicroLabelScore.F0.Should().BeApproximately(0.66667, 0.00001);

            testSubject.MacroLabelScore.Accuracy.Should().Be(0.75);
            testSubject.MacroLabelScore.Precision.Should().BeApproximately(0.66667, 0.00001);
            testSubject.MacroLabelScore.Recall.Should().Be(0.8);
            testSubject.MacroLabelScore.F0.Should().Be(0.625);
        }

        [Fact]
        public void ModelScorerCumulativeScoresOneSample()
        {
            var scorer = new ModelScorer();
            scorer.Eval("one", "one");

            var testSubject = CumulativeScore.Create(scorer.Scores().ToArray());

            testSubject.MicroLabelScore.Accuracy.Should().Be(1.0);
            testSubject.MicroLabelScore.Precision.Should().Be(1.0);
            testSubject.MicroLabelScore.Recall.Should().Be(1.0);
            testSubject.MicroLabelScore.F0.Should().Be(1.0);

            testSubject.MacroLabelScore.Accuracy.Should().Be(1.0);
            testSubject.MacroLabelScore.Precision.Should().Be(1.0);
            testSubject.MacroLabelScore.Recall.Should().Be(1.0);
            testSubject.MacroLabelScore.F0.Should().Be(1.0);
        }
    }
}
