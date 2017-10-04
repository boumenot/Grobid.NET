using System;
using System.Linq;
using ApprovalTests;
using ApprovalTests.Reporters;
using Grobid.NET;
using Grobid.NET.Feature;
using Grobid.NET.Feature.Date;
using Xunit;

namespace Grobid.Test.Feature.Date
{
    [UseReporter(typeof(DiffReporter))]
    public class DateFeatureVectorFactoryTest
    {
        [Fact]
        public void DateFeatureExtractionTest()
        {
            var blockFactory = new RawBlockStateFactory();
            var blocks = blockFactory.CreateWithoutPunctuation("17 Nov 2009");

            var featureVectorFactory = new DateFeatureVectorFactory(
                new FeatureExtractor(EmptyLexicon.Instance));

            var featureVectors = blocks
                .Select(featureVectorFactory.Create)
                .ToArray();

            Approvals.VerifyAll(featureVectors, x => String.Join(" ", DateFeatureFormatter.Instance.Format(x)));
        }

        [Fact]
        [Trait("Category", "EndToEnd")]
        public void DateFeatureExtractionWapitiModelTest()
        {
            var blockFactory = new RawBlockStateFactory();
            var blocks = blockFactory.CreateWithoutPunctuation("17 Nov 2009");

            var featureVectorFactory = new DateFeatureVectorFactory(
                new FeatureExtractor(EmptyLexicon.Instance));

            var featureVectors = blocks
                .Select(featureVectorFactory.Create)
                .ToArray();

            using (var model = global::Wapiti.Wapiti.Load(@"content\models\date\model.wapiti"))
            {
                var lines = featureVectors.Select(x => String.Join(" ", DateFeatureFormatter.Instance.Format(x))).ToArray();
                Approvals.Verify(model.Label(lines));
            }
        }
    }
}
