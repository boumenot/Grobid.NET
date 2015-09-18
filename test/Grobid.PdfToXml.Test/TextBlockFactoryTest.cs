using FluentAssertions;
using Xunit;

namespace Grobid.PdfToXml.Test
{
    public class TextBlockFactoryTest
    {
        [Fact]
        public void TestBlockShouldConcatenateBlocksOnSameLine()
        {
            var tokenBlocks = new[]
            {
                TokenBlock.Empty,
                new TokenBlock { Text = "The" },
                TokenBlock.Empty,
                new TokenBlock { Text = "End" },
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
                TokenBlock.Empty,
                new TokenBlock { Text = "The" },
                TokenBlock.Empty,
                new TokenBlock { Text = "End" },
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
                new TokenBlock { Text = "The" },
                TokenBlock.Empty,
                new TokenBlock { Text = "End" },
                TokenBlock.Empty,
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
                new TokenBlock { Base = 1, Text = "The" },
                TokenBlock.Empty,
                new TokenBlock { Base = 1, Text = "Start" },
                TokenBlock.Empty,
                new TokenBlock { Base = 2, Text = "The" },
                TokenBlock.Empty,
                new TokenBlock { Base = 2, Text = "Finish" },
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

            textBlocks[0].Text.Should().Be("The essence of language-integrated query");
            textBlocks[1].Text.Should().Be("James Cheney");
        }

        [Fact]
        public void BlockProperties()
        {
            var pageBlockFactory = new PageBlockFactory();
            var pageBlocks = pageBlockFactory.Create(Sample.Pdf.OpenEssenseLinq());

            var pageBlock = pageBlocks[0];
            pageBlock.Height.Should().Be(792);
            pageBlock.Width.Should().Be(612);
            pageBlock.Offset.Should().Be(1);

            var textBlock = pageBlocks[0].TextBlocks[0];
            textBlock.Height.Should().BeInRange(16.12f, 16.13f);
            textBlock.Width.Should().BeInRange(323.95f, 323.96f);
            textBlock.X.Should().BeInRange(143.08f, 143.09f);
            textBlock.Y.Should().BeInRange(78.95f, 78.96f);

            var tokenBlock = textBlock.TokenBlocks[0];
            tokenBlock.Text.Should().Be("The");
            tokenBlock.Angle.Should().Be(0);
            tokenBlock.FontName.Name.Should().Be("NimbusRomNo9L");
            tokenBlock.FontColor.Should().Be("#000000");
            tokenBlock.FontSize.Should().BeInRange(17.92f, 17.94f);
            tokenBlock.Height.Should().BeInRange(16.11f, 16.13f);
            tokenBlock.IsBold.Should().BeFalse();
            tokenBlock.IsItalic.Should().BeFalse();
            tokenBlock.IsSymbolic.Should().BeTrue();
            tokenBlock.Rotation.Should().Be(0);
            tokenBlock.Width.Should().BeInRange(29.89f, 29.9f);
            tokenBlock.X.Should().BeInRange(143.07f, 143.09f);
            tokenBlock.Y.Should().BeInRange(78.95f, 79.10f);
            tokenBlock.Base.Should().BeInRange(91.32f, 91.34f);
        }
    }
}
