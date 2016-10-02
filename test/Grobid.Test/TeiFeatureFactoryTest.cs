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
        public void TeiFeatureFactoryAbstract00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<div type=\"abstract\">Abstract <lb/>The advent and acceptance of massively parallel <lb/>machines has made it increasingly important to have <lb/>tools to analyze the performance of programs running on these machines. Current day performance <lb/>tools suffer from two drawbacks: they are not scalable <lb/>and they lose specific information about the user program in their attempt for generality. In this paper, <lb/>we present Projections, a scalable performance tool, <lb/>for Charm that can provide program-specific information to help the users better understand the behavior <lb/>of their programs. <lb/></div>");

            var testSubject = new TeiFeatureFactory();
            Approvals.Verify(testSubject.Create(XDocument.Parse(tei)));
        }

        [Fact]
        public void TeiFeatureFactoryAddress00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<address>Urbana, IL 61801 Urbana, IL 61801 <lb/></address>");

            var testSubject = new TeiFeatureFactory();
            Approvals.Verify(testSubject.Create(XDocument.Parse(tei)));
        }

        [Fact]
        public void TeiFeatureFactoryAffiliation00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<byline><affiliation>Department of Computer Science Department of Computer Science <lb/>University of Illinois University of Illinois <lb/></affiliation></byline>");

            var testSubject = new TeiFeatureFactory();
            Approvals.Verify(testSubject.Create(XDocument.Parse(tei)));
        }

        [Fact]
        public void TeiFeatureFactoryAuthor00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<byline><docAuthor>Amitabh B.Sinha Laxmikant V. Kale<lb/></docAuthor></byline>");

            var testSubject = new TeiFeatureFactory();
            Approvals.Verify(testSubject.Create(XDocument.Parse(tei)));
        }

        [Fact]
        public void TeiFeatureFactoryEmail00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<email>email: sinha@cs.uiuc.edu </email> <email>email: kale@cs.uiuc.edu <lb/></email>");

            var testSubject = new TeiFeatureFactory();
            Approvals.Verify(testSubject.Create(XDocument.Parse(tei)));
        }

        [Fact]
        public void TeiFeatureFactoryIntro00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<div type=\"intro\">1 Introduction<lb/></div>");

            var testSubject = new TeiFeatureFactory();
            Approvals.Verify(testSubject.Create(XDocument.Parse(tei)));
        }

        [Fact]
        public void TeiFeatureFactoryIntro01()
        {
            var tei = this.InsertXmlSnippetIntoTei("<div type=\"introduction\">1 Introduction<lb/></div>");

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
