using FluentAssertions;
using Xunit;

using Grobid.NET;
using Grobid.PdfToXml;

namespace Grobid.Test
{
    public partial class BlockStateFactoryTest
    {
        private static readonly FontName FontA = FontName.Parse("CHUFSU+NimbusRomNo9L-Medi");
        private static readonly FontName FontB = FontName.Parse("CHUFSU+NimbusRomNo9L-Bold");

        [Fact]
        public void InitialFontStatusTest()
        {
            var tokenBlock = new TokenBlock { FontName = BlockStateFactoryTest.FontA };
            var textBlock = new TextBlock(new [] { tokenBlock });
            var block = new Block { TextBlocks = new [] { textBlock } };

            var testSubject = new BlockStateFactory();
            var state = testSubject.Create(block, textBlock, tokenBlock);

            state.FontStatus.Should().Be(FontStatus.NEWFONT);
        }

        [Fact]
        public void NewFontStatusTest()
        {
            var tokenBlock1 = new TokenBlock { FontName = BlockStateFactoryTest.FontA };
            var tokenBlock2 = new TokenBlock { FontName = BlockStateFactoryTest.FontB };
            var textBlock = new TextBlock(new[] { tokenBlock1, tokenBlock2 });
            var block = new Block { TextBlocks = new[] { textBlock } };

            var testSubject = new BlockStateFactory();
            testSubject.Create(block, textBlock, tokenBlock1);
            var state = testSubject.Create(block, textBlock, tokenBlock2);

            state.FontStatus.Should().Be(FontStatus.NEWFONT);
        }

        [Fact]
        public void SameFontStatusTest()
        {
            var tokenBlock1 = new TokenBlock { FontName = BlockStateFactoryTest.FontA };
            var tokenBlock2 = new TokenBlock { FontName = BlockStateFactoryTest.FontA };
            var textBlock = new TextBlock(new[] { tokenBlock1, tokenBlock2 });
            var block = new Block { TextBlocks = new[] { textBlock } };

            var testSubject = new BlockStateFactory();
            testSubject.Create(block, textBlock, tokenBlock1);
            var state = testSubject.Create(block, textBlock, tokenBlock2);

            state.FontStatus.Should().Be(FontStatus.SAMEFONT);
        }
    }
}
