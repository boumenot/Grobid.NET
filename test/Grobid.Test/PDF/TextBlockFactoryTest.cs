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
            var testInfoFactory = new TextInfoFactory(1);
            var textInfoPages = testInfoFactory.Create(Sample.Pdf.OpenEssenseLinq());

            textInfoPages.Should().HaveCount(1);

            var textInfos = textInfoPages.First();
            textInfos.Count.Should().Be(1835);
        }
    }
}
