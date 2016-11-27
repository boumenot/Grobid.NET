using FluentAssertions;

using Grobid.NET.Feature;
using Grobid.PdfToXml;

using Xunit;

namespace Grobid.Test.Feature
{
    public partial class BlockStateFactoryTest
    {
        [Fact]
        public void LineStatusInitialTest()
        {
            var tokenBlock = new TokenBlock { FontName = Feature.BlockStateFactoryTest.FontA };
            var textBlock = new TextBlock(new [] { tokenBlock });
            var block = new Block { TextBlocks = new [] { textBlock } };

            var testSubject = new BlockStateFactory();
            var state = testSubject.Create(block, textBlock, tokenBlock);

            state.LineStatus.Should().Be(LineStatus.LINESTART);
        }

        [Fact]
        public void LineStatusInTest()
        {
            var tokenBlock1 = new TokenBlock { Text = "LineStart", FontName = Feature.BlockStateFactoryTest.FontA };
            var tokenBlock2 = new TokenBlock { Text = "LineIn", FontName = Feature.BlockStateFactoryTest.FontA };
            var tokenBlock3 = new TokenBlock { Text = "LineEnd", FontName = Feature.BlockStateFactoryTest.FontA };
            var textBlock = new TextBlock(new[] { tokenBlock1, tokenBlock2, tokenBlock3 });
            var block = new Block { TextBlocks = new[] { textBlock } };

            var testSubject = new BlockStateFactory();
            var state1 = testSubject.Create(block, textBlock, tokenBlock1);
            var state2 = testSubject.Create(block, textBlock, tokenBlock2);
            var state3 = testSubject.Create(block, textBlock, tokenBlock3);

            state1.LineStatus.Should().Be(LineStatus.LINESTART);
            state2.LineStatus.Should().Be(LineStatus.LINEIN);
            state3.LineStatus.Should().Be(LineStatus.LINEEND);
        }

        [Fact]
        public void LineStatusAcrossTextBlocksTest()
        {
            var tokenBlock1 = new TokenBlock { Text = "TextBlock[0]: LineStart", FontName = Feature.BlockStateFactoryTest.FontA };
            var tokenBlock2 = new TokenBlock { Text = "TextBlock[0]: LineEnd", FontName = Feature.BlockStateFactoryTest.FontA };

            var tokenBlock3 = new TokenBlock { Text = "TextBlock[1]: LineStart", FontName = Feature.BlockStateFactoryTest.FontA };
            var tokenBlock4 = new TokenBlock { Text = "TextBlock[1]: LineEnd", FontName = Feature.BlockStateFactoryTest.FontA };

            var textBlock1 = new TextBlock(new[] { tokenBlock1, tokenBlock2 });
            var textBlock2 = new TextBlock(new[] { tokenBlock3, tokenBlock4 });
            var block = new Block { TextBlocks = new[] { textBlock1, textBlock2 } };

            var testSubject = new BlockStateFactory();
            var state1 = testSubject.Create(block, textBlock1, tokenBlock1);
            var state2 = testSubject.Create(block, textBlock1, tokenBlock2);

            var state3 = testSubject.Create(block, textBlock2, tokenBlock3);
            var state4 = testSubject.Create(block, textBlock2, tokenBlock4);

            state1.LineStatus.Should().Be(LineStatus.LINESTART);
            state2.LineStatus.Should().Be(LineStatus.LINEEND);
            state3.LineStatus.Should().Be(LineStatus.LINESTART);
            state4.LineStatus.Should().Be(LineStatus.LINEEND);
        }

        [Fact]
        public void LineStatusEndTest()
        {
            var tokenBlock1 = new TokenBlock {  Text = "LineStart", FontName = Feature.BlockStateFactoryTest.FontA };
            var tokenBlock2 = new TokenBlock {  Text = "LineEnd", FontName = Feature.BlockStateFactoryTest.FontA };
            var textBlock = new TextBlock(new[] { tokenBlock1, tokenBlock2 });
            var block = new Block { TextBlocks = new[] { textBlock } };

            var testSubject = new BlockStateFactory();
            testSubject.Create(block, textBlock, tokenBlock1);
            var state = testSubject.Create(block, textBlock, tokenBlock2);

            state.LineStatus.Should().Be(LineStatus.LINEEND);
        }

        /// <summary>
        /// If there's a single layout, text, and block confirm that LineStatus is LINESTART.
        /// </summary>
        [Fact]
        public void LineStatusSingleBlockTest()
        {
            var tokenBlock1 = new TokenBlock { Text = "Line", FontName = Feature.BlockStateFactoryTest.FontA };
            var textBlock1 = new TextBlock(new[] { tokenBlock1 });
            var block1 = new Block { TextBlocks = new[] { textBlock1 } };

            var testSubject = new BlockStateFactory();
            var state1 = testSubject.Create(block1, textBlock1, tokenBlock1);

            state1.LineStatus.Should().Be(LineStatus.LINESTART);
        }
    }
}
