using System.Xml.Linq;

using ApprovalTests;
using ApprovalTests.Reporters;
using Xunit;

using Grobid.NET;

namespace Grobid.Test
{
    [UseReporter(typeof(DiffReporter))]
    public class TeiFeatureFactoryTest
    {
        [Fact]
        public void TeiFeatureFactory00()
        {
            var tei = @"<tei>
  <teiHeader>
    <fileDesc xml:id='316'/>
  </teiHeader>
  <text xml:lang='en'>
    <front>
      <docTitle>
	<titlePart type='main'>Iterative Optimization and Simplification of <lb/>Hierarchical Clusterings <lb/></titlePart>
      </docTitle>
      <idno>Technical Report CS-95-01 <lb/></idno>
      <byline><docAuthor>Doug Fisher <lb/></docAuthor></byline>
      <byline><affiliation>Department of Computer Science <lb/></affiliation></byline>
      <address>Box 1679, Station B <lb/></address>
      <byline><affiliation>Vanderbilt University <lb/></affiliation></byline>
      <address>Nashville, TN 37235 <lb/></address>
      <email>dfisher@vuse.vanderbilt.edu <lb/></email>
      <ptr type='web'>http://www.vuse.vanderbilt.edu/~dfisher/dfisher.html <lb/></ptr>
      <note type='phone'>(615) 343-4111 <lb/></note>
      <div type='abstract'>Abstract: Clustering is often used for discovering structure in data. Clustering systems <lb/>differ in the objective function used to evaluate clustering quality and the control strategy <lb/>used to search the space of clusterings. Ideally, the search strategy should consistently <lb/>construct clusterings of high quality, but be computationally inexpensive as well. In general, <lb/>we cannot have it both ways, but we can partition the search so that a system inexpensively <lb/>constructs a `tentative&apos; clustering for initial examination, followed by iterative optimization, <lb/>which continues to search in background for improved clusterings. Given this motivation, we <lb/>evaluate an inexpensive strategy for creating initial clusterings, coupled with several control <lb/>strategies for iterative optimization, each of which repeatedly modifies an initial clustering <lb/>in search of a better one. One of these methods appears novel as an iterative optimization <lb/>strategy in clustering contexts. Once a clustering has been constructed it is judged by <lb/>analysts often according to task-specific criteria. Several authors have abstracted these <lb/>criteria and posited a generic performance task akin to pattern completion, where the error <lb/>rate over completed patterns is used to &apos;externally&apos; judge clustering utility. Given this <lb/>performance task we adapt resampling-based pruning strategies used by supervised learning <lb/>systems to the task of simplifying hierarchical clusterings, thus promising to ease post-clustering analysis. Finally, we propose a number of objective functions, based on attribute-selection measures for decision-tree induction, that might perform well on the error rate and <lb/>simplicity dimensions. <lb/></div>
      <keywords>Keywords: clustering, iterative optimization, cluster validation, resampling, pruning, objective functions. <lb/></keywords>
      <pb/>
    </front>
  </text>
</tei>";

            var testSubject = new TeiFeatureFactory();
            var formatter = new TeiFeatureFormatter();

            Approvals.Verify(formatter.CreateString(testSubject.Create(XDocument.Parse(tei))));
        }

        [Fact]
        public void TeiFeatureFactoryAbstract00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<div type=\"abstract\">Abstract <lb/>The advent and acceptance of massively parallel <lb/>machines has made it increasingly important to have <lb/>tools to analyze the performance of programs running on these machines. Current day performance <lb/>tools suffer from two drawbacks: they are not scalable <lb/>and they lose specific information about the user program in their attempt for generality. In this paper, <lb/>we present Projections, a scalable performance tool, <lb/>for Charm that can provide program-specific information to help the users better understand the behavior <lb/>of their programs. <lb/></div>");

            var testSubject = new TeiFeatureFactory();
            var formatter = new TeiFeatureFormatter();

            Approvals.Verify(formatter.CreateString(testSubject.Create(XDocument.Parse(tei))));
        }

        [Fact]
        public void TeiFeatureFactoryAddress00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<address>Urbana, IL 61801 Urbana, IL 61801 <lb/></address>");

            var testSubject = new TeiFeatureFactory();
            var formatter = new TeiFeatureFormatter();

            Approvals.Verify(formatter.CreateString(testSubject.Create(XDocument.Parse(tei))));
        }

        [Fact]
        public void TeiFeatureFactoryAffiliation00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<byline><affiliation>Department of Computer Science Department of Computer Science <lb/>University of Illinois University of Illinois <lb/></affiliation></byline>");

            var testSubject = new TeiFeatureFactory();
            var formatter = new TeiFeatureFormatter();

            Approvals.Verify(formatter.CreateString(testSubject.Create(XDocument.Parse(tei))));
        }

        [Fact]
        public void TeiFeatureFactoryAuthor00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<byline><docAuthor>Amitabh B.Sinha Laxmikant V. Kale<lb/></docAuthor></byline>");

            var testSubject = new TeiFeatureFactory();
            var formatter = new TeiFeatureFormatter();

            Approvals.Verify(formatter.CreateString(testSubject.Create(XDocument.Parse(tei))));
        }

        [Fact]
        public void TeiFeatureFactoryCopyright00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<note type=\"copyright\">Copyright c fl1997 Mark Lillibridge <lb/></note>");

            var testSubject = new TeiFeatureFactory();
            var formatter = new TeiFeatureFormatter();

            Approvals.Verify(formatter.CreateString(testSubject.Create(XDocument.Parse(tei))));
        }

        [Fact]
        public void TeiFeatureFactoryCopyright01()
        {
            var tei = this.InsertXmlSnippetIntoTei("<note type=\"copyrights\">┬⌐ 2004 Elsevier Ireland Ltd. All rights reserved.<lb /></note>");

            var testSubject = new TeiFeatureFactory();
            var formatter = new TeiFeatureFormatter();

            Approvals.Verify(formatter.CreateString(testSubject.Create(XDocument.Parse(tei))));
        }

        [Fact]
        public void TeiFeatureFactoryDate00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<date>February 1991 <lb/></date>");

            var testSubject = new TeiFeatureFactory();
            var formatter = new TeiFeatureFormatter();

            Approvals.Verify(formatter.CreateString(testSubject.Create(XDocument.Parse(tei))));
        }

        [Fact]
        public void TeiFeatureFactoryDate01()
        {
            var tei = this.InsertXmlSnippetIntoTei("<date type=\"submission\">19 November 2009<lb /></date>");

            var testSubject = new TeiFeatureFactory();
            var formatter = new TeiFeatureFormatter();

            Approvals.Verify(formatter.CreateString(testSubject.Create(XDocument.Parse(tei))));
        }

        [Fact]
        public void TeiFeatureFactoryDedication00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<note type=\"dedication\">Dedicated to H. Hintenberger on the occasion of his 70th birth<lb /></note>");

            var testSubject = new TeiFeatureFactory();
            var formatter = new TeiFeatureFormatter();

            Approvals.Verify(formatter.CreateString(testSubject.Create(XDocument.Parse(tei))));
        }

        [Fact]
        public void TeiFeatureFactoryDegree00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<note type=\"degree\">Submitted in partial fulfillment of the requirements <lb/>for the degree of Doctor of Philosophy. <lb/>Thesis Committee: <lb/>Robert Harper, Chair <lb/>Peter Lee <lb/>John Reynolds <lb/>Luca Cardelli, DEC SRC <lb/></note>");

            var testSubject = new TeiFeatureFactory();
            var formatter = new TeiFeatureFormatter();

            Approvals.Verify(formatter.CreateString(testSubject.Create(XDocument.Parse(tei))));
        }

        [Fact]
        public void TeiFeatureFactoryDegree01()
        {
            var tei = this.InsertXmlSnippetIntoTei("<degree>This report is a reset version of a masters thesis submitted to the Department of Electrical <lb />Engineering and Computer Science on May 15, 1994 in partial fulfillment of the requirements <lb />for the degree of Master of Engineering. <lb /></degree>");

            var testSubject = new TeiFeatureFactory();
            var formatter = new TeiFeatureFormatter();

            Approvals.Verify(formatter.CreateString(testSubject.Create(XDocument.Parse(tei))));
        }

        [Fact]
        public void TeiFeatureFactoryEmail00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<email>email: sinha@cs.uiuc.edu </email> <email>email: kale@cs.uiuc.edu <lb/></email>");

            var testSubject = new TeiFeatureFactory();
            var formatter = new TeiFeatureFormatter();

            Approvals.Verify(formatter.CreateString(testSubject.Create(XDocument.Parse(tei))));
        }

        [Fact]
        public void TeiFeatureFactoryEnTitle00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<note type=\"english-title\">Simultaneous Measurement<lb /> of Hyperfine Structure, Stark-Effect<lb /> and Zeeman-Effect<lb /> of 87 RbF<lb /> with a Molecular<lb /> Beam Resonance<lb /> Apparatus<lb /></note>");

            var testSubject = new TeiFeatureFactory();
            var formatter = new TeiFeatureFormatter();

            Approvals.Verify(formatter.CreateString(testSubject.Create(XDocument.Parse(tei))));
        }

        [Fact]
        public void TeiFeatureFactoryGrant00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<note type=\"grant\">This research was sponsored by the Air Force Materiel Command (AFMC) and the Defense Advanced Research Projects Agency (DARPA) under contract number, F19628-95-C-0050. </note>");

            var testSubject = new TeiFeatureFactory();
            var formatter = new TeiFeatureFormatter();

            Approvals.Verify(formatter.CreateString(testSubject.Create(XDocument.Parse(tei))));
        }

        [Fact]
        public void TeiFeatureFactoryKeyword00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<keywords>Keywords: projective reconstruction, affine reconstruction, partial calibration, qualitative depth<lb/></keywords>");

            var testSubject = new TeiFeatureFactory();
            var formatter = new TeiFeatureFormatter();

            Approvals.Verify(formatter.CreateString(testSubject.Create(XDocument.Parse(tei))));
        }

        [Fact]
        public void TeiFeatureFactoryIntro00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<div type=\"intro\">1 Introduction<lb/></div>");

            var testSubject = new TeiFeatureFactory();
            var formatter = new TeiFeatureFormatter();

            Approvals.Verify(formatter.CreateString(testSubject.Create(XDocument.Parse(tei))));
        }

        [Fact]
        public void TeiFeatureFactoryIntro01()
        {
            var tei = this.InsertXmlSnippetIntoTei("<div type=\"introduction\">1 Introduction<lb/></div>");

            var testSubject = new TeiFeatureFactory();
            var formatter = new TeiFeatureFormatter();

            Approvals.Verify(formatter.CreateString(testSubject.Create(XDocument.Parse(tei))));
        }

        [Fact]
        public void TeiFeatureFactoryNote00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<note>Articles<lb /></note>");

            var testSubject = new TeiFeatureFactory();
            var formatter = new TeiFeatureFormatter();

            Approvals.Verify(formatter.CreateString(testSubject.Create(XDocument.Parse(tei))));
        }

        [Fact]
        public void TeiFeatureFactoryNote01()
        {
            var tei = this.InsertXmlSnippetIntoTei("<note type=\"will-not-match-known-type\">Articles<lb /></note>");

            var testSubject = new TeiFeatureFactory();
            var formatter = new TeiFeatureFormatter();

            Approvals.Verify(formatter.CreateString(testSubject.Create(XDocument.Parse(tei))));
        }

        [Fact]
        public void TeiFeatureFactoryOther00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<note type=\"other\">The U.S. Government is authorized to reproduce and distribute reprints for Government purposes notwithstanding <lb/>any copyright notation thereon. <lb/>The views and conclusions contained in this document are those of the author and should not be <lb/>interpreted as representing the official policies or endorsements, either expressed or implied, of the U.S. <lb/>Government. <lb/></note>");

            var testSubject = new TeiFeatureFactory();
            var formatter = new TeiFeatureFormatter();

            Approvals.Verify(formatter.CreateString(testSubject.Create(XDocument.Parse(tei))));
        }

        [Fact]
        public void TeiFeatureFactoryPhone00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<note type=\"phone\">(615) 343-4111 <lb/></note>");

            var testSubject = new TeiFeatureFactory();
            var formatter = new TeiFeatureFormatter();

            Approvals.Verify(formatter.CreateString(testSubject.Create(XDocument.Parse(tei))));
        }

        [Fact]
        public void TeiFeatureFactoryPubNum00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<idno>Technical Report CS91-13 <lb/></idno>");

            var testSubject = new TeiFeatureFactory();
            var formatter = new TeiFeatureFormatter();

            Approvals.Verify(formatter.CreateString(testSubject.Create(XDocument.Parse(tei))));
        }

        [Fact]
        public void TeiFeatureFactoryReference00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<note type = \"reference\" > Proceedings of the 4th KRDB Workshop<lb /> Athens, Greece, 30-August-1997 <lb />(F.Baader, M.A.Jeusfeld, W.Nutt, eds.) <lb /></note>");

            var testSubject = new TeiFeatureFactory();
            var formatter = new TeiFeatureFormatter();

            Approvals.Verify(formatter.CreateString(testSubject.Create(XDocument.Parse(tei))));
        }

        [Fact]
        public void TeiFeatureFactoryReference01()
        {
            var tei = this.InsertXmlSnippetIntoTei("<reference>Methods 53 (2011) 347ΓÇô355<lb /></reference>");

            var testSubject = new TeiFeatureFactory();
            var formatter = new TeiFeatureFormatter();

            Approvals.Verify(formatter.CreateString(testSubject.Create(XDocument.Parse(tei))));
        }

        [Fact]
        public void TeiFeatureFactorySubmission00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<note type=\"submission\">Submitted to the Graduate School of the <lb/></note>");

            var testSubject = new TeiFeatureFactory();
            var formatter = new TeiFeatureFormatter();

            Approvals.Verify(formatter.CreateString(testSubject.Create(XDocument.Parse(tei))));
        }

        [Fact]
        public void TeiFeatureFactoryTitle00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<docTitle><titlePart>The wizard quickly jinxed the gnomes before they vaporized</titlePart></docTitle>");

            var testSubject = new TeiFeatureFactory();
            var formatter = new TeiFeatureFormatter();

            Approvals.Verify(formatter.CreateString(testSubject.Create(XDocument.Parse(tei))));
        }

        [Fact]
        public void TeiFeatureFactoryTitle01()
        {
            var tei = this.InsertXmlSnippetIntoTei("<docTitle><titlePart type='main'>The wizard quickly jinxed the gnomes before they vaporized</titlePart></docTitle>");

            var testSubject = new TeiFeatureFactory();
            var formatter = new TeiFeatureFormatter();

            Approvals.Verify(formatter.CreateString(testSubject.Create(XDocument.Parse(tei))));
        }

        [Fact]
        public void TeiFeatureFactoryWeb00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<ptr type = \"web\" > http://ciir.cs.umass.edu/ <lb/></ptr>");

            var testSubject = new TeiFeatureFactory();
            var formatter = new TeiFeatureFormatter();

            Approvals.Verify(formatter.CreateString(testSubject.Create(XDocument.Parse(tei))));
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
