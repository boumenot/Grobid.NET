using FluentAssertions;
using Grobid.NET;
using Xunit;

namespace Grobid.Test
{
    public class TextBlockFactoryTest
    {
        [Fact]
        public void Test()
        {
            var pageBlockFactory = new PageInfoFactory();
            var pageBlocks = pageBlockFactory.Create(Sample.Pdf.OpenEssenseLinq());

            var testSubject = new TextBlockFactory();
            var textBlocks = testSubject.Create(pageBlocks[0].TextInfos, pageBlocks[0].Height);
            textBlocks.Should().HaveCount(118);
        }
    }
}
