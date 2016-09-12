using System.Linq;

using ApprovalTests;
using ApprovalTests.Reporters;
using Newtonsoft.Json;
using Xunit;

using Grobid.NET;
using Grobid.PdfToXml;

namespace Grobid.Test
{
    [UseReporter(typeof(DiffReporter))]
    public class HeaderFeatureVectorTest
    {
        [Fact]
        public void HeaderFeatureExtractionTest()
        {
            var testInfoFactory = new PageBlockFactory();
            var pageBlocks = testInfoFactory.Create(Sample.Pdf.OpenEssenseLinq(), 1);

            var blocks = pageBlocks.SelectMany(x => x.Blocks);

            var featureExtractor = new FeatureExtractor(EmptyLexicon.Instance);
            var headerFeatureVectorFactory = new HeaderFeatureVectorFactory(featureExtractor);

            var testSubject = new PdfBlockExtractor<HeaderFeatureVector>();
            var featureVectors = testSubject.Extract(blocks, x => headerFeatureVectorFactory.Create(x));

            Approvals.VerifyJson(JsonConvert.SerializeObject(featureVectors));
        }
    }
}
