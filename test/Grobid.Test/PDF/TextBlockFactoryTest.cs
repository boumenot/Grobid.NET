using System.Linq;

using FluentAssertions;
using Grobid.NET;
using Xunit;

namespace Grobid.Test.PDF
{
    public class TextBlockFactoryTest
    {
        [Fact]
        public void Test()
        {
            var testInfoFactory = new PageInfoFactory(1);
            var pageBlocks = testInfoFactory.Create(Sample.Pdf.OpenEssenseLinq());

            pageBlocks.Should().HaveCount(1);

            var textInfos = pageBlocks[0].TextInfos;
            textInfos.Should().HaveCount(1835);
        }
    }
}
