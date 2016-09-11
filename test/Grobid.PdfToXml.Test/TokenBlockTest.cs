using System.Linq;

using FluentAssertions;
using iTextSharp.text;
using Xunit;

namespace Grobid.PdfToXml.Test
{
    public class TokenBlockTest
    {
        [Fact]
        public void CreateEmptyShouldReturnInstance()
        {
            var testSubject = TokenBlock.Empty;
            testSubject.Should().NotBeNull();
        }

        [Fact]
        public void CreateEmptyTextBlockIsEmpty()
        {
            var testSubject = TokenBlock.Empty;
            testSubject.IsEmpty.Should().BeTrue();
        }

        [Fact]
        public void MergeShouldElongateRectangle()
        {
            var boundingRectangle1 = new Rectangle(0, 1, 1, 2);
            var boundingRectangle2 = new Rectangle(1, 1, 2, 2);
            var boundingRectangle3 = new Rectangle(2, 1, 3, 2);

            var tokenBlocks = new[]
            {
                new TokenBlock { Base = 1, Text = "Lan", BoundingRectangle = boundingRectangle1},
                new TokenBlock { Base = 1, Text = "gua", BoundingRectangle = boundingRectangle2},
                new TokenBlock { Base = 1, Text = "ge",  BoundingRectangle = boundingRectangle3 },
            };

            var testSubject = TokenBlock.Merge(tokenBlocks);
            testSubject.Text.Should().Be("Language");

            testSubject.BoundingRectangle.Left.Should().Be(0);
            testSubject.BoundingRectangle.Bottom.Should().Be(1);
            testSubject.BoundingRectangle.Top.Should().Be(2);
            testSubject.BoundingRectangle.Right.Should().Be(3);
        }

        [Fact]
        public void MergeShouldIncreaseWidth()
        {
            var boundingRectangle1 = new Rectangle(0, 1, 1, 2);
            var boundingRectangle2 = new Rectangle(1, 1, 6, 2);

            var tokenBlocks = new[]
            {
                new TokenBlock { Base = 1, Text = "ob",    BoundingRectangle = boundingRectangle1, Width = 1.3f },
                new TokenBlock { Base = 1, Text = "vious", BoundingRectangle = boundingRectangle2, Width = 4.9f },
            };

            var testSubject = TokenBlock.Merge(tokenBlocks);
            testSubject.Text.Should().Be("obvious");
            testSubject.BoundingRectangle.Width.Should().Be(6);
            testSubject.Width.Should().Be(6);
        }

        [Fact]
        public void MergeShouldNormalizeTextStrings()
        {
            var boundingRectangle1 = new Rectangle(0, 1, 1, 2);
            var boundingRectangle2 = new Rectangle(1, 1, 6, 2);

            var tokenBlocks = new[]
            {
                new TokenBlock { Base = 1, Text = "abcd\u0065", BoundingRectangle = boundingRectangle1, Width = 1.3f },
                new TokenBlock { Base = 1, Text = "\u0301fgh",  BoundingRectangle = boundingRectangle2, Width = 4.9f },
            };

            var testSubject = TokenBlock.Merge(tokenBlocks);
            testSubject.Text.ToCharArray().Should().HaveCount(8);
        }

        [Fact]
        public void TokenizeTokenBlock()
        {
            var boundingRectangle1 = new Rectangle(0, 1, 2, 3);

            var tokenBlock = new TokenBlock
            {
                BoundingRectangle = boundingRectangle1,
                Width = 1.1f,
                Height = 2.2f,
                Angle = 135,
                Base = 3.3f,
                Text = "abc,def.",
                FontColor = "#012345",
                FontFlags = FontFlags.Bold,
                FontSize = 5.5f,
                IsEmpty = false,
                X = 6.6f,
                Y = 7.7f,
            };

            var tokenBlocks = tokenBlock.Tokenize();
            tokenBlocks.Should().HaveCount(4);

            tokenBlocks[0].Text.Should().Be("abc");
            tokenBlocks[1].Text.Should().Be(",");
            tokenBlocks[2].Text.Should().Be("def");
            tokenBlocks[3].Text.Should().Be(".");

            tokenBlocks.All(x => x.BoundingRectangle.Equals(boundingRectangle1)).Should().BeTrue();
            tokenBlocks.All(x => x.Width == 1.1f).Should().BeTrue();
            tokenBlocks.All(x => x.Height == 2.2f).Should().BeTrue();
            tokenBlocks.All(x => x.Angle == 135).Should().BeTrue();
            tokenBlocks.All(x => x.Base == 3.3f).Should().BeTrue();
            tokenBlocks.All(x => x.FontColor == "#012345").Should().BeTrue();
            tokenBlocks.All(x => x.FontFlags == FontFlags.Bold).Should().BeTrue();
            tokenBlocks.All(x => x.FontSize == 5.5f).Should().BeTrue();
            tokenBlocks.All(x => !x.IsEmpty).Should().BeTrue();
            tokenBlocks.All(x => x.X == 6.6f).Should().BeTrue();
            tokenBlocks.All(x => x.Y == 7.7f).Should().BeTrue();
        }

        [Fact]
        public void TokenizeEmptyText()
        {
            var boundingRectangle1 = new Rectangle(0, 1, 2, 3);

            var tokenBlock = new TokenBlock
            {
                BoundingRectangle = boundingRectangle1,
                Width = 1.1f,
                Height = 2.2f,
                Angle = 135,
                Base = 3.3f,
                Text = "",
                FontColor = "#012345",
                FontFlags = FontFlags.Bold,
                FontSize = 5.5f,
                IsEmpty = false,
                X = 6.6f,
                Y = 7.7f,
            };

            var tokenBlocks = tokenBlock.Tokenize();
            tokenBlocks.Should().HaveCount(1);

            tokenBlocks[0].Text.Should().Be("");

            tokenBlocks.All(x => x.BoundingRectangle.Equals(boundingRectangle1)).Should().BeTrue();
            tokenBlocks.All(x => x.Width == 1.1f).Should().BeTrue();
            tokenBlocks.All(x => x.Height == 2.2f).Should().BeTrue();
            tokenBlocks.All(x => x.Angle == 135).Should().BeTrue();
            tokenBlocks.All(x => x.Base == 3.3f).Should().BeTrue();
            tokenBlocks.All(x => x.FontColor == "#012345").Should().BeTrue();
            tokenBlocks.All(x => x.FontFlags == FontFlags.Bold).Should().BeTrue();
            tokenBlocks.All(x => x.FontSize == 5.5f).Should().BeTrue();
            tokenBlocks.All(x => !x.IsEmpty).Should().BeTrue();
            tokenBlocks.All(x => x.X == 6.6f).Should().BeTrue();
            tokenBlocks.All(x => x.Y == 7.7f).Should().BeTrue();

        }

        [Fact]
        public void TokenizeSingleToken()
        {
            var boundingRectangle1 = new Rectangle(0, 1, 2, 3);

            var tokenBlock = new TokenBlock
            {
                BoundingRectangle = boundingRectangle1,
                Width = 1.1f,
                Height = 2.2f,
                Angle = 135,
                Base = 3.3f,
                Text = "IsOneToken",
                FontColor = "#012345",
                FontFlags = FontFlags.Bold,
                FontSize = 5.5f,
                IsEmpty = false,
                X = 6.6f,
                Y = 7.7f,
            };

            var tokenBlocks = tokenBlock.Tokenize();
            tokenBlocks.Should().HaveCount(1);

            tokenBlocks[0].Text.Should().Be("IsOneToken");

            tokenBlocks.All(x => x.BoundingRectangle.Equals(boundingRectangle1)).Should().BeTrue();
            tokenBlocks.All(x => x.Width == 1.1f).Should().BeTrue();
            tokenBlocks.All(x => x.Height == 2.2f).Should().BeTrue();
            tokenBlocks.All(x => x.Angle == 135).Should().BeTrue();
            tokenBlocks.All(x => x.Base == 3.3f).Should().BeTrue();
            tokenBlocks.All(x => x.FontColor == "#012345").Should().BeTrue();
            tokenBlocks.All(x => x.FontFlags == FontFlags.Bold).Should().BeTrue();
            tokenBlocks.All(x => x.FontSize == 5.5f).Should().BeTrue();
            tokenBlocks.All(x => !x.IsEmpty).Should().BeTrue();
            tokenBlocks.All(x => x.X == 6.6f).Should().BeTrue();
            tokenBlocks.All(x => x.Y == 7.7f).Should().BeTrue();
        }

        [Fact]
        public void EqualityTest()
        {
            var tokenBlock1 = new TokenBlock {  X = 1.0f, Y = 1.0f, Width = 2.0f, Height = 2.0f, Text = "tbA" };
            var tokenBlock2 = new TokenBlock {  X = 1.0f, Y = 1.0f, Width = 2.0f, Height = 2.0f, Text = "tbA" };
            var tokenBlock3 = new TokenBlock {  X = 1.0f, Y = 1.0f, Width = 2.0f, Height = 2.0f, Text = "tbB" };

            tokenBlock1.Should().Be(tokenBlock2);
            tokenBlock1.GetHashCode().Should().Be(tokenBlock2.GetHashCode());

            tokenBlock1.Should().NotBe(tokenBlock3);
            tokenBlock1.GetHashCode().Should().NotBe(tokenBlock3.GetHashCode());
        }
    }
}
