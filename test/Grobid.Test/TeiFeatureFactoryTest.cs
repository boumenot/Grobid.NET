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
        public void TeiFeatureFactoryCopyright00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<note type=\"copyright\">Copyright c fl1997 Mark Lillibridge <lb/></note>");

            var testSubject = new TeiFeatureFactory();
            Approvals.Verify(testSubject.Create(XDocument.Parse(tei)));
        }

        [Fact]
        public void TeiFeatureFactoryCopyright01()
        {
            var tei = this.InsertXmlSnippetIntoTei("<note type=\"copyrights\">┬⌐ 2004 Elsevier Ireland Ltd. All rights reserved.<lb /></note>");

            var testSubject = new TeiFeatureFactory();
            Approvals.Verify(testSubject.Create(XDocument.Parse(tei)));
        }

        [Fact]
        public void TeiFeatureFactoryDate00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<date>February 1991 <lb/></date>");

            var testSubject = new TeiFeatureFactory();
            Approvals.Verify(testSubject.Create(XDocument.Parse(tei)));
        }

        [Fact]
        public void TeiFeatureFactoryDate01()
        {
            var tei = this.InsertXmlSnippetIntoTei("<date type=\"submission\">19 November 2009<lb /></date>");

            var testSubject = new TeiFeatureFactory();
            Approvals.Verify(testSubject.Create(XDocument.Parse(tei)));
        }

        [Fact]
        public void TeiFeatureFactoryDedication00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<note type=\"dedication\">Dedicated to H. Hintenberger on the occasion of his 70th birth<lb /></note>");

            var testSubject = new TeiFeatureFactory();
            Approvals.Verify(testSubject.Create(XDocument.Parse(tei)));
        }

        [Fact]
        public void TeiFeatureFactoryDegree00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<note type=\"degree\">Submitted in partial fulfillment of the requirements <lb/>for the degree of Doctor of Philosophy. <lb/>Thesis Committee: <lb/>Robert Harper, Chair <lb/>Peter Lee <lb/>John Reynolds <lb/>Luca Cardelli, DEC SRC <lb/></note>");

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
        public void TeiFeatureFactoryEnTitle00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<note type=\"english-title\">Simultaneous Measurement<lb /> of Hyperfine Structure, Stark-Effect<lb /> and Zeeman-Effect<lb /> of 87 RbF<lb /> with a Molecular<lb /> Beam Resonance<lb /> Apparatus<lb /></note>");

            var testSubject = new TeiFeatureFactory();
            Approvals.Verify(testSubject.Create(XDocument.Parse(tei)));
        }

        [Fact]
        public void TeiFeatureFactoryGrant00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<note type=\"grant\">This research was sponsored by the Air Force Materiel Command (AFMC) and the Defense Advanced Research Projects Agency (DARPA) under contract number, F19628-95-C-0050. </note>");

            var testSubject = new TeiFeatureFactory();
            Approvals.Verify(testSubject.Create(XDocument.Parse(tei)));
        }

        [Fact]
        public void TeiFeatureFactoryKeyword00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<keywords>Keywords: projective reconstruction, affine reconstruction, partial calibration, qualitative depth<lb/></keywords>");

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
        public void TeiFeatureFactoryOther00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<note type=\"other\">The U.S. Government is authorized to reproduce and distribute reprints for Government purposes notwithstanding <lb/>any copyright notation thereon. <lb/>The views and conclusions contained in this document are those of the author and should not be <lb/>interpreted as representing the official policies or endorsements, either expressed or implied, of the U.S. <lb/>Government. <lb/></note>");

            var testSubject = new TeiFeatureFactory();
            Approvals.Verify(testSubject.Create(XDocument.Parse(tei)));
        }

        [Fact]
        public void TeiFeatureFactoryPhone00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<note type=\"phone\">(615) 343-4111 <lb/></note>");

            var testSubject = new TeiFeatureFactory();
            Approvals.Verify(testSubject.Create(XDocument.Parse(tei)));
        }

        [Fact]
        public void TeiFeatureFactoryPubNum00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<idno>Technical Report CS91-13 <lb/></idno>");

            var testSubject = new TeiFeatureFactory();
            Approvals.Verify(testSubject.Create(XDocument.Parse(tei)));
        }

        [Fact]
        public void TeiFeatureFactorySubmission00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<note type=\"submission\">Submitted to the Graduate School of the <lb/></note>");

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

        [Fact]
        public void TeiFeatureFactoryWeb00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<ptr type = \"web\" > http://ciir.cs.umass.edu/ <lb/></ptr>");

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
