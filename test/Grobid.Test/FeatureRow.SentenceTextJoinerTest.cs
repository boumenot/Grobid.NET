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

        [Fact]
        public void Punctuation()
        {
            var testSubject = new SentenceTextJoiner();
            var featureRows = new[]
            {
                new FeatureRow() { Classification = "title", Value = "The" },
                new FeatureRow() { Classification = "title", Value = "dog" },
                new FeatureRow() { Classification = "title", Value = "is" },
                new FeatureRow() { Classification = "title", Value = "brown" },
                new FeatureRow() { Classification = "title", Value = "." },
            };

            var title = testSubject.Join(featureRows);
            title.Should().Be("The dog is brown.");
        }
    }

    public class SentenceTextJoiner
    {
        public string Join(FeatureRow[] featureRows)
        {
            var sb = new StringBuilder();
            foreach (var featureRow in featureRows)
            {
                if (featureRow.Value == ".")
                {
                    sb.Replace(' ', '.', sb.Length - 1, 1);
                }
                else
                {
                    sb.Append(featureRow.Value);
                }

                sb.Append(" ");
            }

            sb.Remove(sb.Length - 1, 1); // trim extraneous whitespace
            return sb.ToString();
        }
    }
}
