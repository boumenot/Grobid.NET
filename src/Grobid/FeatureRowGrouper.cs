using System;
using System.Collections.Generic;
using System.Linq;

namespace Grobid.NET
{
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