using System;
using System.Linq;
using System.Xml.Linq;

using ApprovalTests;
using ApprovalTests.Reporters;
using ApprovalUtilities.Utilities;
using Xunit;

using Grobid.NET;
using Grobid.NET.Feature;
using Grobid.NET.Feature.Date;
using Grobid.Test.Feature;

namespace Grobid.Test.Tei
{
    [UseReporter(typeof(DiffReporter))]
    public class DateFeatureFactoryTest
    {
        [Fact]
        public void DateFeatureFactoryTest00()
        {
            var tei = @"<dates><date><month>March</month> <day>28</day>, <year>2007</year></date></dates>";

            var factory = new DateFeatureVectorFactory(
                new FeatureExtractor(EmptyLexicon.Instance));

            var formatter = new LabeledFeatureFormatter<DateFeatureVector>(
                new DateFeatureFormatter());

            var testSubject = new DateFeatureFactory(factory);
            Approvals.VerifyAll(
                testSubject.Create(XDocument.Parse(tei)),
                x => String.Join("\n", x.Select(y => formatter.Format(y.FeatureVector, y).Join(" "))) + "\n");
        }

        [Fact]
        public void DateFeatureFactoryTest01()
        {
            var tei = @"<dates>
  <date><month>March</month> <day>28</day>, <year>2007</year></date>
  <date>Published <day>25</day> <month>May</month> <year>2011</year></date>
</dates>";

            var factory = new DateFeatureVectorFactory(
                new FeatureExtractor(EmptyLexicon.Instance));

            var formatter = new LabeledFeatureFormatter<DateFeatureVector>(
                new DateFeatureFormatter());

            var testSubject = new DateFeatureFactory(factory);
            Approvals.VerifyAll(
                testSubject.Create(XDocument.Parse(tei)),
                x => String.Join("\n", x.Select(y => formatter.Format(y.FeatureVector, y).Join(" "))) + "\n");
        }
    }
}
