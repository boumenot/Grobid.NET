using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;
using Xunit;

using Grobid.NET;

namespace Grobid.Test
{
    public class HeaderModelFactoryTest
    {
        [Fact(Skip="Need to implement more")]
        public void Test()
        {
            var featureRows = new[]
            {
                new FeatureRow { IsStart = true, Classification = "email", Value = "email1" },
                new FeatureRow { IsStart = true, Classification = "email", Value = "email2" },
            };

            var testSubject = new HeaderModelFactory();
            var model = testSubject.Create(new FeatureRow[0]);

            model.Title.Should().Be("The essence of language-integrated query");
            model.Keywords.Should().HaveCount(5);
            model.Keywords[0].Should().Be("lambda calculus");
            model.Keywords[1].Should().Be("LINQ");
            model.Keywords[2].Should().Be("F#");
            model.Keywords[3].Should().Be("quotation");
            model.Keywords[4].Should().Be("anti-quotation");

            model.Authors.Should().HaveCount(3);

            model.Authors[0].Name.Should().Be("James Cheney");
            model.Authors[0].Affiliation.Should().Be("University of Edinburgh");
            model.Authors[0].EMail.Should().Be("jcheney@inf.ed.ac.uk");

            model.Authors[1].Name.Should().Be("Sam Lindley");
            model.Authors[1].Affiliation.Should().Be("University of Strathclyde");
            model.Authors[1].EMail.Should().Be("sam.lindley@strath.ac.uk");

            model.Authors[2].Name.Should().Be("Philip Wadler");
            model.Authors[2].Affiliation.Should().Be("University of Edinburgh");
            model.Authors[2].EMail.Should().Be("wadler@inf.ed.ac.uk");
        }

        [Fact]
        public void Test1()
        {
            var featureRows = new[]
            {
                new FeatureRow { Classification = "title", Value = "The" },
                new FeatureRow { Classification = "title", Value = "essence" },
                new FeatureRow { Classification = "title", Value = "of" },
                new FeatureRow { Classification = "title", Value = "language" },
                new FeatureRow { Classification = "title", Value = "-" },
                new FeatureRow { Classification = "title", Value = "integrated" },
                new FeatureRow { Classification = "title", Value = "query" },

                new FeatureRow { Classification = "author", Value = "James" },
                new FeatureRow { Classification = "author", Value = "Cheney" },

                new FeatureRow { Classification = "affiliation", Value = "University" },
                new FeatureRow { Classification = "affiliation", Value = "of" },
                new FeatureRow { Classification = "affiliation", Value = "Edinburgh" },

                new FeatureRow { Classification = "email", Value = "jcheney@inf" },
                new FeatureRow { Classification = "email", Value = "." },
                new FeatureRow { Classification = "email", Value = "ed" },
                new FeatureRow { Classification = "email", Value = "." },
                new FeatureRow { Classification = "email", Value = "ac" },
                new FeatureRow { Classification = "email", Value = "." },
                new FeatureRow { Classification = "email", Value = "uk" },

                new FeatureRow { Classification = "author", Value = "Sam" },
                new FeatureRow { Classification = "author", Value = "Lindley" },

                new FeatureRow { Classification = "affiliation", Value = "University" },
                new FeatureRow { Classification = "affiliation", Value = "of" },
                new FeatureRow { Classification = "affiliation", Value = "Strathclyde" },

                new FeatureRow { Classification = "email", Value = "sam" },
                new FeatureRow { Classification = "email", Value = "." },
                new FeatureRow { Classification = "email", Value = "lindley@strath" },
                new FeatureRow { Classification = "email", Value = "." },
                new FeatureRow { Classification = "email", Value = "ac" },
                new FeatureRow { Classification = "email", Value = "." },
                new FeatureRow { Classification = "email", Value = "uk" },
            };

            var testSubject = new HeaderModelFactory();
            var model = testSubject.Create(featureRows);

            model.Title.Should().Be("The essence of language-integrated query");
            model.Authors.Should().HaveCount(2);

            model.Authors[0].Name.Should().Be("James Cheney");
            model.Authors[0].Affiliation.Should().Be("University of Edinburgh");
            model.Authors[0].EMail.Should().Be("jcheney@inf.ed.ac.uk");

            model.Authors[1].Name.Should().Be("Sam Lindley");
            model.Authors[1].Affiliation.Should().Be("University of Strathclyde");
            model.Authors[1].EMail.Should().Be("sam.lindley@strath.ac.uk");
        }
    }

    public class HeaderModelFactory
    {
        private readonly FeatureRowGrouper grouper = new FeatureRowGrouper();
        private readonly FeatureRowTextJoiner textJoiner = new FeatureRowTextJoiner();
        private readonly SentenceTextJoiner sentenceJoiner = new SentenceTextJoiner();

        private static readonly FeatureRow[] Empty = new FeatureRow[0];

        public HeaderModel Create(FeatureRow[] featureRows)
        {
            var model = new HeaderModel();

            var groups = this.grouper.Group(featureRows);
            this.ProcessTitle(groups, model);
            this.ProcessAuthors(groups, model);

            return model;
        }

        private FeatureRow[] GetByClassification(ArraySegment<FeatureRow>[] groups, string classification)
        {
            var rows = groups
                .FirstOrDefault(x => x.First().Classification == classification)
                .ToArray();

            return rows;
        }

        private void ProcessTitle(ArraySegment<FeatureRow>[] groups, HeaderModel model)
        {
            var rows = this.GetByClassification(groups, Constants.Classification.Title);
            model.Title = this.sentenceJoiner.Join(rows);
        }

        private void ProcessAuthors(ArraySegment<FeatureRow>[] groups, HeaderModel model)
        {
            var d = new Dictionary<string, FeatureRow[]>();

            foreach (var g in groups)
            {
                var classification = g.First().Classification;

                if (classification != Constants.Classification.Author &&
                    classification != Constants.Classification.Affiliation &&
                    classification != Constants.Classification.Email)
                {
                    continue;
                }

                if (d.ContainsKey(classification))
                {
                    this.ProcessAuthor(d, model);
                    d.Clear();
                }

                d[classification] = g.ToArray();
            }

            if (d.Any())
            {
                this.ProcessAuthor(d, model);
            }
        }

        private void ProcessAuthor(Dictionary<string, FeatureRow[]> authorFeatureRows, HeaderModel model)
        {
            var author = new AuthorModel();

            foreach (var classification in authorFeatureRows.Keys)
            {
                switch (classification)
                {
                    case Constants.Classification.Author:
                        author.Name = this.sentenceJoiner.Join(authorFeatureRows[classification]);
                        break;
                    case Constants.Classification.Affiliation:
                        author.Affiliation = this.sentenceJoiner.Join(authorFeatureRows[classification]);
                        break;
                    case Constants.Classification.Email:
                        author.EMail = this.textJoiner.Join(authorFeatureRows[classification]);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"The classification {classification} is not supported.");
                }
            }

            model.Authors.Add(author);
        }
    }

    public class HeaderModel
    {
        public HeaderModel()
        {
            this.Authors = new List<AuthorModel>();
            this.Keywords = new List<string>();
        }

        public string Title { get; set; }
        public string Abstract { get; set; }
        public List<AuthorModel> Authors { get; }
        public List<string> Keywords { get; }
    }

    public class AuthorModel
    {
        public string Name { get; set; }
        public string Affiliation { get; set; }
        public string EMail { get; set; }
    }
}
