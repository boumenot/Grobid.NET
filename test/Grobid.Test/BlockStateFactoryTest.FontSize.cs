using FluentAssertions;
using Xunit;

using Grobid.NET;
using Grobid.PdfToXml;

namespace Grobid.Test
{
    public partial class BlockStateFactoryTest
    {
        [Fact]
        public void InitialFontSizeTest()
        {
            var tokenBlock = new TokenBlock { FontName = BlockStateFactoryTest.FontA, FontSize = 3.0f };
            var textBlock = new TextBlock(new [] { tokenBlock });
            var block = new Block { TextBlocks = new [] { textBlock } };

            var testSubject = new BlockStateFactory();
            var state = testSubject.Create(block, textBlock, tokenBlock);

            state.FontSizeStatus.Should().Be(FontSizeStatus.HIGHERFONT);
        }

        [Fact]
        public void FontSizeLargerTest()
        {
            var tokenBlock1 = new TokenBlock { FontName = BlockStateFactoryTest.FontA, FontSize = 3.0f };
            var tokenBlock2 = new TokenBlock { FontName = BlockStateFactoryTest.FontB, FontSize = 4.0f };
            var textBlock = new TextBlock(new[] { tokenBlock1, tokenBlock2 });
            var block = new Block { TextBlocks = new[] { textBlock } };

            var testSubject = new BlockStateFactory();
            testSubject.Create(block, textBlock, tokenBlock1);
            var state = testSubject.Create(block, textBlock, tokenBlock2);

            state.FontSizeStatus.Should().Be(FontSizeStatus.HIGHERFONT);
        }

        [Fact]
        public void FontSizeStatusSmallerTest()
        {
            var tokenBlock1 = new TokenBlock { FontName = BlockStateFactoryTest.FontA, FontSize = 3.0f };
            var tokenBlock2 = new TokenBlock { FontName = BlockStateFactoryTest.FontB, FontSize = 2.0f };
            var textBlock = new TextBlock(new[] { tokenBlock1, tokenBlock2 });
            var block = new Block { TextBlocks = new[] { textBlock } };

            var testSubject = new BlockStateFactory();
            testSubject.Create(block, textBlock, tokenBlock1);
            var state = testSubject.Create(block, textBlock, tokenBlock2);

            state.FontSizeStatus.Should().Be(FontSizeStatus.LOWFONT);
        }

        [Fact]
        public void FontSizeStatusSameTest()
        {
            var tokenBlock1 = new TokenBlock { FontName = BlockStateFactoryTest.FontA, FontSize = 3.0f };
            var tokenBlock2 = new TokenBlock { FontName = BlockStateFactoryTest.FontB, FontSize = 3.0f };
            var textBlock = new TextBlock(new[] { tokenBlock1, tokenBlock2 });
            var block = new Block { TextBlocks = new[] { textBlock } };

            var testSubject = new BlockStateFactory();
            testSubject.Create(block, textBlock, tokenBlock1);
            var state = testSubject.Create(block, textBlock, tokenBlock2);

            state.FontSizeStatus.Should().Be(FontSizeStatus.SAMEFONTSIZE);
        }
    }
}
