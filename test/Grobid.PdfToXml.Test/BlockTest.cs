using FluentAssertions;
using Xunit;

namespace Grobid.PdfToXml.Test
{
    public class BlockTest
    {
        [Fact]
        public void Test00()
        {
            var tokenBlock1 = new TokenBlock { Text = "The lazy dog" };
            var tokenBlock2 = new TokenBlock { Text = "jumped over the" };
            var textBlock1 = new TextBlock(new[] { tokenBlock1 });
            var textBlock2 = new TextBlock(new[] { tokenBlock2 });

            var testSubject = new Block
            {
                TextBlocks = new [] { textBlock1, textBlock2 },
                Page = 0,
            };

            testSubject.Text.Should().Be("The lazy dog\njumped over the");
        }
    }
}
