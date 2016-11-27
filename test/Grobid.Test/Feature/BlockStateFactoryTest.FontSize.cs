using FluentAssertions;

using Grobid.NET.Feature;
using Grobid.PdfToXml;

using Xunit;

namespace Grobid.Test.Feature
{
    public partial class BlockStateFactoryTest
    {
        [Fact]
        public void FontSizeStatusInitialTest()
        {
            var tokenBlock = new TokenBlock { FontName = Feature.BlockStateFactoryTest.FontA, FontSize = 3.0f };
            var textBlock = new TextBlock(new [] { tokenBlock });
            var block = new Block { TextBlocks = new [] { textBlock } };

            var testSubject = new BlockStateFactory();
            var state = testSubject.Create(block, textBlock, tokenBlock);

            state.FontSizeStatus.Should().Be(FontSizeStatus.HIGHERFONT);
            state.FontName.FullName.Should().Be(Feature.BlockStateFactoryTest.FontA.FullName);
        }

        [Fact]
        public void FontSizeStatusLargerTest()
        {
            var tokenBlock1 = new TokenBlock { FontName = Feature.BlockStateFactoryTest.FontA, FontSize = 3.0f };
            var tokenBlock2 = new TokenBlock { FontName = Feature.BlockStateFactoryTest.FontB, FontSize = 4.0f };
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
            var tokenBlock1 = new TokenBlock { FontName = Feature.BlockStateFactoryTest.FontA, FontSize = 3.0f };
            var tokenBlock2 = new TokenBlock { FontName = Feature.BlockStateFactoryTest.FontB, FontSize = 2.0f };
            var textBlock = new TextBlock(new[] { tokenBlock1, tokenBlock2 });
            var block = new Block { TextBlocks = new[] { textBlock } };

            var testSubject = new BlockStateFactory();
            testSubject.Create(block, textBlock, tokenBlock1);
            var state = testSubject.Create(block, textBlock, tokenBlock2);

            state.FontSizeStatus.Should().Be(FontSizeStatus.LOWERFONT);
        }

        [Fact]
        public void FontSizeStatusSameTest()
        {
            var tokenBlock1 = new TokenBlock { FontName = Feature.BlockStateFactoryTest.FontA, FontSize = 3.0f };
            var tokenBlock2 = new TokenBlock { FontName = Feature.BlockStateFactoryTest.FontB, FontSize = 3.0f };
            var textBlock = new TextBlock(new[] { tokenBlock1, tokenBlock2 });
            var block = new Block { TextBlocks = new[] { textBlock } };

            var testSubject = new BlockStateFactory();
            testSubject.Create(block, textBlock, tokenBlock1);
            var state = testSubject.Create(block, textBlock, tokenBlock2);

            state.FontSizeStatus.Should().Be(FontSizeStatus.SAMEFONTSIZE);
        }
    }
}
