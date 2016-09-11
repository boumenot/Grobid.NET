using FluentAssertions;
using Xunit;

using Grobid.NET;
using Grobid.PdfToXml;

namespace Grobid.Test
{
    public partial class BlockStateFactoryTest
    {
        [Fact]
        public void BlockStartTest()
        {
            var tokenBlock = new TokenBlock();
            var textBlock = new TextBlock(new [] { tokenBlock });
            var block = new Block { TextBlocks = new [] { textBlock } };

            var testSubject = new BlockStateFactory();
            var state = testSubject.Create(block, textBlock, tokenBlock);

            state.BlockStatus.Should().Be(BlockStatus.BLOCKSTART);
        }

        [Fact]
        public void BlockInTest()
        {
            var tokenBlock1 = new TokenBlock { Text = "LineStart" };
            var tokenBlock2 = new TokenBlock { Text = "LineIn" };
            var textBlock = new TextBlock(new[] { tokenBlock1, tokenBlock2 });
            var block = new Block { TextBlocks = new[] { textBlock } };

            var testSubject = new BlockStateFactory();
            testSubject.Create(block, textBlock, tokenBlock1);
            var state = testSubject.Create(block, textBlock, tokenBlock2);

            state.BlockStatus.Should().Be(BlockStatus.BLOCKIN);
        }

        [Fact]
        public void BlockEndTest()
        {
            var tokenBlock = new TokenBlock {  Text = "LineStart" };
            var textBlock = new TextBlock(new[] { tokenBlock });
            var block1 = new Block { TextBlocks = new[] { textBlock } };
            var block2 = new Block { TextBlocks = new[] { textBlock } };

            var testSubject = new BlockStateFactory();
            testSubject.Create(block1, textBlock, tokenBlock);
            var state = testSubject.Create(block2, textBlock, tokenBlock);

            state.BlockStatus.Should().Be(BlockStatus.BLOCKEND);
        }
    }
}
