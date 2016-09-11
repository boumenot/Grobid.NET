using ApprovalTests;
using ApprovalTests.Reporters;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Grobid.PdfToXml.Test
{
    [UseReporter(typeof(DiffReporter))]
    public class PageBlockFactoryTest
    {
        [Fact]
        public void Test()
        {
            var testInfoFactory = new PageBlockFactory();
            var pageBlocks = testInfoFactory.Create(Sample.Pdf.OpenEssenseLinq(), 1);

            pageBlocks.Should().HaveCount(1);
            pageBlocks[0].Offset.Should().Be(1);

            Approvals.VerifyJson(JsonConvert.SerializeObject(pageBlocks));
        }
    }
}
