using System.Linq;
using System.Xml.Linq;

using ApprovalTests;
using ApprovalTests.Reporters;
using Xunit;

using Grobid.NET;
using Grobid.NET.Feature.Date;

namespace Grobid.Test.Tei
{
    [UseReporter(typeof(DiffReporter))]
    public class DateFeatureFactoryTest
    {
        [Fact]
        public void DateFeatureFactoryTest00()
        {
            var tei = @"<dates><date><month>March</month> <day>28</day>, <year>2007</year></date></dates>";

            var testSubject = new DateFeatureFactory();
            var formatter = new TeiFeatureFormatter();

            Approvals.Verify(formatter.CreateString(testSubject.Create(XDocument.Parse(tei)).First()));
        }

        [Fact]
        public void DateFeatureFactoryTest01()
        {
            var tei = @"<dates>
  <date><month>March</month> <day>28</day>, <year>2007</year></date>
  <date>Published <day>25</day> <month>May</month> <year>2011</year></date>
</dates>";

            var testSubject = new DateFeatureFactory();
            var formatter = new TeiFeatureFormatter();

            Approvals.VerifyAll(
                testSubject.Create(XDocument.Parse(tei)),
                x => formatter.CreateString(x) + "\n");
        }
    }
}
