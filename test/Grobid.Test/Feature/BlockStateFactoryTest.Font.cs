using FluentAssertions;

using Grobid.NET.Feature;
using Grobid.PdfToXml;

using Xunit;

namespace Grobid.Test.Feature
{
    public partial class BlockStateFactoryTest
    {
        private static readonly FontName FontA = FontName.Parse("CHUFSU+NimbusRomNo9L-Medi");
        private static readonly FontName FontB = FontName.Parse("CHUFSU+NimbusRomNo9L-Bold");

        [Fact]
        public void FontStatusInitialTest()
        {
            var tokenBlock = new TokenBlock { FontName = Feature.BlockStateFactoryTest.FontA };
            var textBlock = new TextBlock(new [] { tokenBlock });
            var block = new Block { TextBlocks = new [] { textBlock } };

            var testSubject = new BlockStateFactory();
            var state = testSubject.Create(block, textBlock, tokenBlock);

            state.FontStatus.Should().Be(FontStatus.NEWFONT);
        }

        [Fact]
        public void FontStatusNewTest()
        {
            var tokenBlock1 = new TokenBlock { FontName = Feature.BlockStateFactoryTest.FontA };
            var tokenBlock2 = new TokenBlock { FontName = Feature.BlockStateFactoryTest.FontB };
            var textBlock = new TextBlock(new[] { tokenBlock1, tokenBlock2 });
            var block = new Block { TextBlocks = new[] { textBlock } };

            var testSubject = new BlockStateFactory();
            testSubject.Create(block, textBlock, tokenBlock1);
            var state = testSubject.Create(block, textBlock, tokenBlock2);

            state.FontStatus.Should().Be(FontStatus.NEWFONT);
        }

        [Fact]
        public void FontStatusSameTest()
        {
            var tokenBlock1 = new TokenBlock { FontName = Feature.BlockStateFactoryTest.FontA };
            var tokenBlock2 = new TokenBlock { FontName = Feature.BlockStateFactoryTest.FontA };
            var textBlock = new TextBlock(new[] { tokenBlock1, tokenBlock2 });
            var block = new Block { TextBlocks = new[] { textBlock } };

            var testSubject = new BlockStateFactory();
            testSubject.Create(block, textBlock, tokenBlock1);
            var state = testSubject.Create(block, textBlock, tokenBlock2);

            state.FontStatus.Should().Be(FontStatus.SAMEFONT);
        }
    }
}
