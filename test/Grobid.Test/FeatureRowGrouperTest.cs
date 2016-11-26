using System;
using System.Collections.Generic;
using System.Linq;

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

    public class FeatureRowGrouper
    {
        public Dictionary<string, List<ArraySegment<FeatureRow>>> Group(FeatureRow[] featureRows)
        {
            var groups = this.Break(featureRows)
                .GroupBy(x => x.Item1)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(y => y.Item2).ToList());

            return groups;
        }

        private IEnumerable<Tuple<string, ArraySegment<FeatureRow>>> Break(FeatureRow[] featureRows)
        {
            int offset = 0;
            while (offset < featureRows.Length)
            {
                var featureRow = featureRows.Skip(offset).First();
                var startingOffset = featureRow.IsStart ? 1 : 0;

                var length = featureRows
                    .Skip(offset + startingOffset)
                    .TakeWhile(x => !x.IsStart && x.Classification == featureRow.Classification)
                    .Count();

                yield return Tuple.Create(
                    featureRow.Classification,
                    new ArraySegment<FeatureRow>(featureRows, offset, length + startingOffset));

                offset += length + startingOffset;
            }
        }
    }
}
