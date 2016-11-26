using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using Grobid.NET;

using Xunit;

namespace Grobid.Test
{
    public class SentenceTextJoinerTest
    {
        [Fact]
        public void SimpleText()
        {
            var testSubject = new SentenceTextJoiner();
            var featureRows = new[]
            {
                new FeatureRow() { Classification = "title", Value = "The" },
                new FeatureRow() { Classification = "title", Value = "Big" },
                new FeatureRow() { Classification = "title", Value = "Bad" },
                new FeatureRow() { Classification = "title", Value = "Title" },
            };

            var title = testSubject.Join(featureRows);
            title.Should().Be("The Big Bad Title");
        }
    }

    public class SentenceTextJoiner
    {
        public string Join(FeatureRow[] featureRows)
        {
            var s = String.Join(" ", featureRows.Select(x => x.Value));
            return s;
        }
    }
}
