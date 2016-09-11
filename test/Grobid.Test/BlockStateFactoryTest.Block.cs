using FluentAssertions;
using Xunit;

using Grobid.NET;
using Grobid.PdfToXml;

namespace Grobid.Test
{
    public partial class BlockStateFactoryTest
    {
        [Fact]
        public void BlockStatusInitialTest()
        {
            var tokenBlock = new TokenBlock { FontName = BlockStateFactoryTest.FontA };
            var textBlock = new TextBlock(new [] { tokenBlock });
            var block = new Block { TextBlocks = new [] { textBlock } };

            var testSubject = new BlockStateFactory();
            var state = testSubject.Create(block, textBlock, tokenBlock);

            state.BlockStatus.Should().Be(BlockStatus.BLOCKSTART);
        }

        [Fact]
        public void BlockStatusTest()
        {
            var tokenBlock1 = new TokenBlock { Text = "LineStart", FontName = BlockStateFactoryTest.FontA };
            var tokenBlock2 = new TokenBlock { Text = "LineIn", FontName = BlockStateFactoryTest.FontA };
            var tokenBlock3 = new TokenBlock { Text = "LineEnd", FontName = BlockStateFactoryTest.FontA };
            var textBlock = new TextBlock(new[] { tokenBlock1, tokenBlock2, tokenBlock3 });
            var block1 = new Block { TextBlocks = new[] { textBlock } };

            var testSubject = new BlockStateFactory();
            var state1 = testSubject.Create(block1, textBlock, tokenBlock1);
            var state2 = testSubject.Create(block1, textBlock, tokenBlock2);
            var state3 = testSubject.Create(block1, textBlock, tokenBlock3);

            state1.BlockStatus.Should().Be(BlockStatus.BLOCKSTART);
            state2.BlockStatus.Should().Be(BlockStatus.BLOCKIN);
            state3.BlockStatus.Should().Be(BlockStatus.BLOCKEND);
        }
    }
}
