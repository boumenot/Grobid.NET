using System;
using System.Collections.Generic;
using System.Linq;

namespace Grobid.NET
{
    public sealed class FeatureRowGrouper
    {
        public ArraySegment<FeatureRow>[] Group(FeatureRow[] featureRows)
        {
            var groups = this.Break(featureRows)
                .ToArray();

            return groups;
        }

        private IEnumerable<ArraySegment<FeatureRow>> Break(FeatureRow[] featureRows)
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

                yield return new ArraySegment<FeatureRow>(featureRows, offset, length + startingOffset);
                offset += length + startingOffset;
            }
        }
    }
}