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

        [Theory]
        [InlineData(".")]
        [InlineData("?")]
        [InlineData("!")]
        public void Punctuation(string punctuation)
        {
            var testSubject = new SentenceTextJoiner();
            var featureRows = new[]
            {
                new FeatureRow() { Classification = "title", Value = "The" },
                new FeatureRow() { Classification = "title", Value = "dog" },
                new FeatureRow() { Classification = "title", Value = "is" },
                new FeatureRow() { Classification = "title", Value = "brown" },
            }.Concat(new[]
                {
                    new FeatureRow() { Classification = "title", Value = punctuation },
                })
            .ToArray();


            var title = testSubject.Join(featureRows);
            title.Should().Be($"The dog is brown{punctuation}");
        }

        [Fact]
        public void Hyphen()
        {
            var testSubject = new SentenceTextJoiner();
            var featureRows = new[]
            {
                new FeatureRow { Classification = "title", Value = "The" },
                new FeatureRow { Classification = "title", Value = "essence" },
                new FeatureRow { Classification = "title", Value = "of" },
                new FeatureRow { Classification = "title", Value = "language" },
                new FeatureRow { Classification = "title", Value = "-" },
                new FeatureRow { Classification = "title", Value = "integrated" },
                new FeatureRow { Classification = "title", Value = "query" },
            };

            var title = testSubject.Join(featureRows);
            title.Should().Be("The essence of language-integrated query");
        }

        [Fact]
        public void ManyHyphens()
        {
            var testSubject = new SentenceTextJoiner();
            var featureRows = new[]
            {
                new FeatureRow { Classification = "title", Value = "The" },
                new FeatureRow { Classification = "title", Value = "-" },
                new FeatureRow { Classification = "title", Value = "essence" },
                new FeatureRow { Classification = "title", Value = "-" },
                new FeatureRow { Classification = "title", Value = "of" },
                new FeatureRow { Classification = "title", Value = "-" },
                new FeatureRow { Classification = "title", Value = "language" },
                new FeatureRow { Classification = "title", Value = "-" },
                new FeatureRow { Classification = "title", Value = "integrated" },
                new FeatureRow { Classification = "title", Value = "-" },
                new FeatureRow { Classification = "title", Value = "query" },
            };

            var title = testSubject.Join(featureRows);
            title.Should().Be("The-essence-of-language-integrated-query");
        }


        [Theory]
        [InlineData(".")]
        [InlineData("?")]
        [InlineData("!")]
        [InlineData("-")]
        public void LeadingPunctuation(string punctuation)
        {
            var testSubject = new SentenceTextJoiner();
            var featureRows = new[]
            {
                new FeatureRow { Classification = "title", Value = punctuation },
            };

            var title = testSubject.Join(featureRows);
            title.Should().Be(punctuation);
        }
    }

    public enum SentenceTextState
    {
        NoSpace,
        Space,
    }

    public class SentenceTextJoiner
    {
        public string Join(FeatureRow[] featureRows)
        {
            var sb = new StringBuilder();
            var state = SentenceTextState.Space;

            foreach (var featureRow in featureRows)
            {
                if (sb.Length > 0 && this.EndsSentence(featureRow.Value))
                {
                    sb.Replace(' ', featureRow.Value[0], sb.Length - 1, 1);
                }
                else if (sb.Length > 0 && featureRow.Value == "-")
                {
                    sb.Replace(' ', featureRow.Value[0], sb.Length - 1, 1);
                    state = SentenceTextState.NoSpace;
                }
                else if (state == SentenceTextState.NoSpace)
                {
                    sb.Remove(sb.Length - 1, 1); // trim extraneous whitespace
                    sb.Append(featureRow.Value);
                    state = SentenceTextState.Space;
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

        private bool EndsSentence(string value)
        {
            return value == "." || value == "?" || value == "!";
        }
    }
}
