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
            var pageBlock = new PageBlock()
            {
                Height = 100,
                TokenBlocks = new[]
                {
                    TokenBlock.Create("The", this.baseline, this.bottomLeft1, this.topRight1),
                    TokenBlock.CreateEmpty(),
                    TokenBlock.Create("End", this.baseline, this.bottomLeft1, this.topRight1),
                },
            };

            var testSubject = new TextBlockFactory();

            var textBlocks = testSubject.Create(pageBlock);
            AssertionExtensions.Should((string)textBlocks[0].Text).Be("The End");
        }

        [Fact]
        public void TrailingEmptyBlocksAreNotTrimmed()
        {
            var pageBlock = new PageBlock()
            {
                Height = 100,
                TokenBlocks = new[]
                {
                    TokenBlock.Create("The", this.baseline, this.bottomLeft1, this.topRight1),
                    TokenBlock.CreateEmpty(),
                    TokenBlock.Create("End", this.baseline, this.bottomLeft1, this.topRight1),
                    TokenBlock.CreateEmpty(),
                },
            };

            var testSubject = new TextBlockFactory();

            var textBlocks = testSubject.Create(pageBlock);
            AssertionExtensions.Should((string)textBlocks[0].Text).Be("The End ");
        }

        [Fact]
        public void TokenBlocksOnDistinctYAxisAreDistinctBlocks()
        {
            var pageBlock = new PageBlock()
            {
                Height = 100,
                TokenBlocks = new[]
                {
                    TokenBlock.Create("The", this.baseline, this.bottomLeft1, this.topRight1),
                    TokenBlock.CreateEmpty(),
                    TokenBlock.Create("Start", this.baseline, this.bottomLeft1, this.topRight1),
                    TokenBlock.Create("The", this.baseline, this.bottomLeft2, this.topRight2),
                    TokenBlock.CreateEmpty(),
                    TokenBlock.Create("Finish", this.baseline, this.bottomLeft2, this.topRight2),
                },
            };

            var testSubject = new TextBlockFactory();

            var textBlocks = testSubject.Create(pageBlock);
            textBlocks.Should().HaveCount(2);
            AssertionExtensions.Should((string)textBlocks[0].Text).Be("The Start");
            AssertionExtensions.Should((string)textBlocks[1].Text).Be("The Finish");
        }

        [Fact]
        public void ParsePdfText()
        {
            var pageBlockFactory = new PageBlockFactory();
            var pageBlocks = pageBlockFactory.Create(Sample.Pdf.OpenEssenseLinq());

            var testSubject = new TextBlockFactory();
            var textBlocks = testSubject.Create(pageBlocks[0]);
            textBlocks.Should().HaveCount(118);

            AssertionExtensions.Should((string)textBlocks[0].Text).Be("The essence of language-integrated query\n");
            AssertionExtensions.Should((string)textBlocks[1].Text).Be("James Cheney\n");
        }

        [Fact]
        public void TokenBlockProperties()
        {
            var pageBlockFactory = new PageBlockFactory();
            var pageBlocks = pageBlockFactory.Create(Sample.Pdf.OpenEssenseLinq());

            var textBlockFactory = new TextBlockFactory();
            var textBlock = textBlockFactory.Create(pageBlocks[0]).First();
            var tokenBlock = textBlock.TokenBlocks[0];

            AssertionExtensions.Should((string)tokenBlock.Text).Be("The");
            //tokenBlock.Angle.Should().Be(0);
            AssertionExtensions.Should((string)tokenBlock.FontName.Name).Be("NimbusRomNo9L");
            AssertionExtensions.Should((string)tokenBlock.FontColor).Be("#000000");
            AssertionExtensions.Should((float)tokenBlock.FontSize).BeInRange(17.92f, 17.94f);
            AssertionExtensions.Should((float)tokenBlock.Height).BeInRange(16.11f, 16.13f);
            AssertionExtensions.Should((bool)tokenBlock.IsBold).BeFalse();
            AssertionExtensions.Should((bool)tokenBlock.IsItalic).BeFalse();
            AssertionExtensions.Should((bool)tokenBlock.IsSymbolic).BeTrue();
            //tokenBlock.Rotation.Should().Be(0);
            AssertionExtensions.Should((float)tokenBlock.Width).BeInRange(29.89f, 29.9f);
            AssertionExtensions.Should((float)tokenBlock.X).BeInRange(143.07f, 143.09f);
            AssertionExtensions.Should((float)tokenBlock.Y).BeInRange(78.95f, 79.10f);
            AssertionExtensions.Should((float)tokenBlock.Base).BeInRange(91.32f, 91.34f);
        }
    }
}
