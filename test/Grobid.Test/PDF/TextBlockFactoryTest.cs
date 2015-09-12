using FluentAssertions;
using Grobid.NET;
using iTextSharp.text.pdf.parser;
using Xunit;

namespace Grobid.Test
{
    public class TextBlockFactoryTest
    {
        public readonly LineSegment baseline = new LineSegment(new Vector(0, 0, 0), new Vector(0, 0, 0));
        public readonly Vector bottomLeft = new Vector(0, 0, 0);
        public readonly Vector topRight = new Vector(0, 0, 0);

        [Fact]
        public void TestBlockShouldConcatenateBlocksOnSameLine()
        {
            var pageBlock = new PageBlock()
            {
                Height = 100,
                TextInfos = new[]
                {
                    TextInfo.Create("The", this.baseline, this.bottomLeft, this.topRight),
                    TextInfo.CreateEmpty(),
                    TextInfo.Create("End", this.baseline, this.bottomLeft, this.topRight),
                },
            };

            var testSubject = new TextBlockFactory();

            var textBlocks = testSubject.Create(pageBlock);
            textBlocks[0].Text.Should().Be("The End");
        }


        [Fact]
        public void ParsePdfText()
        {
            var pageBlockFactory = new PageBackFactory();
            var pageBlocks = pageBlockFactory.Create(Sample.Pdf.OpenEssenseLinq());

            var testSubject = new TextBlockFactory();
            var textBlocks = testSubject.Create(pageBlocks[0]);
            textBlocks.Should().HaveCount(118);

            textBlocks[0].Text.Should().Be("The essence of language-integrated query\n");
            textBlocks[1].Text.Should().Be("James Cheney\n");
        }
    }
}
