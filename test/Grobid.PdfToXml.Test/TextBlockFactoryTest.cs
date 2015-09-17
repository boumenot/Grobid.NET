using System.Linq;

using FluentAssertions;
using iTextSharp.text.pdf.parser;
using Xunit;

namespace Grobid.PdfToXml.Test
{
    public class TextBlockFactoryTest
    {
        public readonly LineSegment baseline = new LineSegment(new Vector(0, 0, 0), new Vector(0, 0, 0));
        public readonly Vector bottomLeft1 = new Vector(0, 0, 0);
        public readonly Vector topRight1 = new Vector(0, 0, 0);
        public readonly Vector bottomLeft2 = new Vector(1, 1, 0);
        public readonly Vector topRight2 = new Vector(1, 1, 0);

        [Fact]
        public void TestBlockShouldConcatenateBlocksOnSameLine()
        {
            var tokenBlocks = new[]
            {
                TokenBlock.Create("The", this.baseline, this.bottomLeft1, this.topRight1),
                TokenBlock.CreateEmpty(),
                TokenBlock.Create("End", this.baseline, this.bottomLeft1, this.topRight1),
            };

            var testSubject = new TextBlockFactory();

            var textBlocks = testSubject.Create(tokenBlocks, 100);
            textBlocks[0].Text.Should().Be("The End");
        }

        [Fact]
        public void LeadingEmptyBlocksShouldBeTrimmed()
        {
            var tokenBlocks = new[]
            {
                TokenBlock.CreateEmpty(),
                TokenBlock.Create("The", this.baseline, this.bottomLeft1, this.topRight1),
                TokenBlock.CreateEmpty(),
                TokenBlock.Create("End", this.baseline, this.bottomLeft1, this.topRight1),
            };

            var testSubject = new TextBlockFactory();

            var textBlocks = testSubject.Create(tokenBlocks, 100);
            textBlocks[0].Text.Should().Be("The End");
        }

        [Fact]
        public void TrailingEmptyBlocksShouldBeTrimmed()
        {
            var tokenBlocks = new[]
            {
                TokenBlock.Create("The", this.baseline, this.bottomLeft1, this.topRight1),
                TokenBlock.CreateEmpty(),
                TokenBlock.Create("End", this.baseline, this.bottomLeft1, this.topRight1),
                TokenBlock.CreateEmpty(),
            };

            var testSubject = new TextBlockFactory();

            var textBlocks = testSubject.Create(tokenBlocks, 100);
            textBlocks[0].Text.Should().Be("The End");
        }

        [Fact]
        public void TokenBlocksOnDistinctYAxisAreDistinctBlocks()
        {
            var tokenBlocks = new[]
            {
                TokenBlock.Create("The", this.baseline, this.bottomLeft1, this.topRight1),
                TokenBlock.CreateEmpty(),
                TokenBlock.Create("Start", this.baseline, this.bottomLeft1, this.topRight1),
                TokenBlock.Create("The", this.baseline, this.bottomLeft2, this.topRight2),
                TokenBlock.CreateEmpty(),
                TokenBlock.Create("Finish", this.baseline, this.bottomLeft2, this.topRight2),
            };

            var testSubject = new TextBlockFactory();

            var textBlocks = testSubject.Create(tokenBlocks, 100);
            textBlocks.Should().HaveCount(2);
            textBlocks[0].Text.Should().Be("The Start");
            textBlocks[1].Text.Should().Be("The Finish");
        }

        [Fact]
        public void ParsePdfText()
        {
            var pageBlockFactory = new PageBlockFactory();
            var pageBlocks = pageBlockFactory.Create(Sample.Pdf.OpenEssenseLinq());
            var textBlocks = pageBlocks[0].TextBlocks;

            textBlocks[0].Text.Should().Be("The essence of language-integrated query\n");
            textBlocks[1].Text.Should().Be("James Cheney\n");
        }

        [Fact]
        public void TokenBlockProperties()
        {
            var pageBlockFactory = new PageBlockFactory();
            var pageBlocks = pageBlockFactory.Create(Sample.Pdf.OpenEssenseLinq());

            var tokenBlock = pageBlocks[0].TextBlocks[0].TokenBlocks[0];

            tokenBlock.Text.Should().Be("The");
            //tokenBlock.Angle.Should().Be(0);
            tokenBlock.FontName.Name.Should().Be("NimbusRomNo9L");
            tokenBlock.FontColor.Should().Be("#000000");
            tokenBlock.FontSize.Should().BeInRange(17.92f, 17.94f);
            tokenBlock.Height.Should().BeInRange(16.11f, 16.13f);
            tokenBlock.IsBold.Should().BeFalse();
            tokenBlock.IsItalic.Should().BeFalse();
            tokenBlock.IsSymbolic.Should().BeTrue();
            //tokenBlock.Rotation.Should().Be(0);
            tokenBlock.Width.Should().BeInRange(29.89f, 29.9f);
            tokenBlock.X.Should().BeInRange(143.07f, 143.09f);
            tokenBlock.Y.Should().BeInRange(78.95f, 79.10f);
            tokenBlock.Base.Should().BeInRange(91.32f, 91.34f);
        }
    }
}
