using FluentAssertions;
using iTextSharp.text.pdf;
using Xunit;

namespace Grobid.Test.PDF
{
    public class EssenseLinqTest
    {
        [Fact]
        public void BasicPropertiesOfPdf()
        {
            var reader = new PdfReader(Sample.Pdf.OpenEssenseLinq());

            reader.FileLength.Should().Be(299258);
            reader.NumberOfPages.Should().Be(13);
            reader.PdfVersion.Should().Be('5');

            reader.GetPageSize(1).Height.Should().Be(792);
            reader.GetPageSize(1).Width.Should().Be(612);
        }
    }
}
