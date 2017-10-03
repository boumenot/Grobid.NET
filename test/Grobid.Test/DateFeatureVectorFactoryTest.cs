using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApprovalTests;
using ApprovalTests.Reporters;
using Grobid.NET;
using Grobid.NET.Feature;
using Grobid.Test.Feature;
using Xunit;

namespace Grobid.Test
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

            Func<DateFeatureVector, string[]> formatter = x =>
            {
                return new[]
                {
                    x.Text,
                    x.AsLowerCase,
                    x.Prefix1,
                    x.Prefix2,
                    x.Prefix3,
                    x.Prefix4,
                    x.Suffix1,
                    x.Suffix2,
                    x.Suffix3,
                    x.Suffix4,
                    x.LineStatus.ToString(),
                    x.Capitalization.ToString(),
                    x.Digit.ToString(),
                    x.IsSingleChar ? "1" : "0",
                    x.IsYear ? "1" : "0",
                    x.IsMonth ? "1" : "0",
                    x.Punctuation.ToString(),
                };
            };

            Approvals.VerifyAll(featureVectors, x => String.Join(" ", formatter(x)));
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

            Func<DateFeatureVector, string[]> formatter = x =>
            {
                return new[]
                {
                    x.Text,
                    x.AsLowerCase,
                    x.Prefix1,
                    x.Prefix2,
                    x.Prefix3,
                    x.Prefix4,
                    x.Suffix1,
                    x.Suffix2,
                    x.Suffix3,
                    x.Suffix4,
                    x.LineStatus.ToString(),
                    x.Capitalization.ToString(),
                    x.Digit.ToString(),
                    x.IsSingleChar ? "1" : "0",
                    x.IsYear ? "1" : "0",
                    x.IsMonth ? "1" : "0",
                    x.Punctuation.ToString(),
                };
            };

            using (var model = global::Wapiti.Wapiti.Load(@"content\models\date\model.wapiti"))
            {
                var lines = featureVectors.Select(x => String.Join(" ", formatter(x))).ToArray();
                Approvals.Verify(model.Label(lines));
            }
        }
    }
}
