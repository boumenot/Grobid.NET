using System.Xml.Linq;

using ApprovalTests;
using ApprovalTests.Reporters;

using Grobid.NET;

using Xunit;

namespace Grobid.Test
{
    [UseReporter(typeof(DiffReporter))]
    public class TeiFeatureFactoryTest
    {
        [Fact]
        public void TeiFeatureFactoryAddress00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<address>Urbana, IL 61801 Urbana, IL 61801 <lb/></address>");

            var testSubject = new TeiFeatureFactory();
            Approvals.Verify(testSubject.Create(XDocument.Parse(tei)));
        }

        [Fact]
        public void TeiFeatureFactoryTitle00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<docTitle><titlePart>The wizard quickly jinxed the gnomes before they vaporized</titlePart></docTitle>");

            var testSubject = new TeiFeatureFactory();
            Approvals.Verify(testSubject.Create(XDocument.Parse(tei)));
        }

        [Fact]
        public void TeiFeatureFactoryTitle01()
        {
            var tei = this.InsertXmlSnippetIntoTei("<docTitle><titlePart type='main'>The wizard quickly jinxed the gnomes before they vaporized</titlePart></docTitle>");

            var testSubject = new TeiFeatureFactory();
            Approvals.Verify(testSubject.Create(XDocument.Parse(tei)));
        }


        private string InsertXmlSnippetIntoTei(string s)
        {
            var tei = $@"
<?xml version='1.0' encoding='utf-8'?>
<tei>
  <text xml:lang='en'>
    <front>
     {s}
    </front>
  </text>
</tei>";
            return tei.Trim();
        }
    }
}
