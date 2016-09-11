using FluentAssertions;
using Xunit;

using Grobid.NET;
using Grobid.PdfToXml;

namespace Grobid.Test
{
    public partial class BlockStateFactoryTest
    {
        [Fact]
        public void LineStatusInitialTest()
        {
            var tokenBlock = new TokenBlock();
            var textBlock = new TextBlock(new [] { tokenBlock });
            var block = new Block { TextBlocks = new [] { textBlock } };

            var testSubject = new BlockStateFactory();
            var state = testSubject.Create(block, textBlock, tokenBlock);

            state.LineStatus.Should().Be(LineStatus.LINESTART);
        }

        [Fact]
        public void LineStatusInTest()
        {
            var tokenBlock1 = new TokenBlock { Text = "LineStart" };
            var tokenBlock2 = new TokenBlock { Text = "LineIn" };
            var tokenBlock3 = new TokenBlock { Text = "LineEnd" };
            var textBlock = new TextBlock(new[] { tokenBlock1, tokenBlock2, tokenBlock3 });
            var block = new Block { TextBlocks = new[] { textBlock } };

            var testSubject = new BlockStateFactory();
            testSubject.Create(block, textBlock, tokenBlock1);
            var state = testSubject.Create(block, textBlock, tokenBlock2);

            state.LineStatus.Should().Be(LineStatus.LINEIN);
        }

        [Fact]
        public void LineStatusEndTest()
        {
            var tokenBlock1 = new TokenBlock {  Text = "LineStart" };
            var tokenBlock2 = new TokenBlock {  Text = "LineEnd" };
            var textBlock = new TextBlock(new[] { tokenBlock1, tokenBlock2 });
            var block = new Block { TextBlocks = new[] { textBlock } };

            var testSubject = new BlockStateFactory();
            testSubject.Create(block, textBlock, tokenBlock1);
            var state = testSubject.Create(block, textBlock, tokenBlock2);

            state.LineStatus.Should().Be(LineStatus.LINEEND);
        }
    }
}
