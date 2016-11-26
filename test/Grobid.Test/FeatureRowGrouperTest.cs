using ApprovalTests;
using ApprovalTests.Reporters;
using Newtonsoft.Json;
using Xunit;

using Grobid.NET;

namespace Grobid.Test
{
    [UseReporter(typeof(BeyondCompare4Reporter))]
    public class FeatureRowGrouperTest
    {
        [Fact]
        public void Simple()
        {
            var testSubject = new FeatureRowGrouper();
            var featureRows = new[]
            {
                new FeatureRow { Classification = "title", Value = "title" },
                new FeatureRow { Classification = "email", Value = "email" },
            };

            var groups = testSubject.Group(featureRows);
            Approvals.VerifyJson(JsonConvert.SerializeObject(groups));
        }

        [Fact]
        public void ConsecutiveClassifications()
        {
            var testSubject = new FeatureRowGrouper();
            var featureRows = new[]
            {
                new FeatureRow { IsStart = true, Classification = "email", Value = "email1" },
                new FeatureRow { IsStart = true, Classification = "email", Value = "email2" },
            };

            var groups = testSubject.Group(featureRows);
            Approvals.VerifyJson(JsonConvert.SerializeObject(groups));
        }

        [Fact]
        public void DifferentClassifications()
        {
            var testSubject = new FeatureRowGrouper();
            var featureRows = new[]
            {
                new FeatureRow { IsStart = true, Classification = "email", Value = "email1" },
                new FeatureRow { Classification = "email", Value = "email1" },
                new FeatureRow { Classification = "email", Value = "email1" },
                new FeatureRow { Classification = "email", Value = "email1" },
                new FeatureRow { IsStart = true, Classification = "title", Value = "title1" },
                new FeatureRow { Classification = "title", Value = "title1" },
                new FeatureRow { Classification = "title", Value = "title1" },
            };

            var groups = testSubject.Group(featureRows);
            Approvals.VerifyJson(JsonConvert.SerializeObject(groups));
        }

        [Fact]
        public void DifferentClassificationsWithoutStart()
        {
            var testSubject = new FeatureRowGrouper();
            var featureRows = new[]
            {
                new FeatureRow { Classification = "email", Value = "email1" },
                new FeatureRow { Classification = "email", Value = "email1" },
                new FeatureRow { Classification = "email", Value = "email1" },
                new FeatureRow { Classification = "email", Value = "email1" },
                new FeatureRow { Classification = "title", Value = "title1" },
                new FeatureRow { Classification = "title", Value = "title1" },
                new FeatureRow { Classification = "title", Value = "title1" },
            };

            var groups = testSubject.Group(featureRows);
            Approvals.VerifyJson(JsonConvert.SerializeObject(groups));
        }
    }
}
