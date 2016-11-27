using FluentAssertions;
using Xunit;

using Grobid.NET;
using Grobid.NET.Model;

namespace Grobid.Test.Model
{
    public class HeaderFactoryTest
    {
        [Fact]
        public void Simple()
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

            var testSubject = new HeaderFactory();
            var model = testSubject.Create(featureRows);

            model.Title.Should().Be("The essence of language-integrated query");
            model.Authors.Should().HaveCount(2);

            model.Authors[0].Name.Should().Be("James Cheney");
            model.Authors[0].Affiliation.Should().Be("University of Edinburgh");
            model.Authors[0].Email.Should().Be("jcheney@inf.ed.ac.uk");

            model.Authors[1].Name.Should().Be("Sam Lindley");
            model.Authors[1].Affiliation.Should().Be("University of Strathclyde");
            model.Authors[1].Email.Should().Be("sam.lindley@strath.ac.uk");
        }

        [Fact]
        public void ValidateBreakInAuthors()
        {
            var featureRows = new[]
            {
                new FeatureRow { Classification = "author", Value = "James Cheney" },
                new FeatureRow { Classification = "affiliation", Value = "University of Edinburgh" },
                new FeatureRow { Classification = "email", Value = "jcheney@inf.ed.ac.uk" },

                new FeatureRow { Classification = "other", Value = "break" },

                new FeatureRow { Classification = "author", Value = "Sam Lindley" },
                new FeatureRow { Classification = "affiliation", Value = "University of Strathclyde" },
                new FeatureRow { Classification = "email", Value = "sam.lindley@strath.ac.uk" },
            };

            var testSubject = new HeaderFactory();
            var model = testSubject.Create(featureRows);

            model.Authors.Should().HaveCount(2);

            model.Authors[0].Name.Should().Be("James Cheney");
            model.Authors[0].Affiliation.Should().Be("University of Edinburgh");
            model.Authors[0].Email.Should().Be("jcheney@inf.ed.ac.uk");

            model.Authors[1].Name.Should().Be("Sam Lindley");
            model.Authors[1].Affiliation.Should().Be("University of Strathclyde");
            model.Authors[1].Email.Should().Be("sam.lindley@strath.ac.uk");
        }

        /// <summary>
        /// When processing Author data the code continues to collect attributes for
        /// the "current" author until a new author's attributes are found.
        /// 
        /// For this test there is the feature author then other.  The feature "other"
        /// does not interrupt the processing of the current author.  The affiliation and
        /// email feature are assumed to attached to previous author feature.
        /// 
        /// I do not know if this behavior is desirable, but I am codifying it in a test 
        /// case.
        /// </summary>
        [Fact]
        public void AuthorAttributesAreCollectedUntilDuplication()
        {
            var featureRows = new[]
            {
                new FeatureRow { Classification = "author", Value = "James Cheney" },

                new FeatureRow { Classification = "other", Value = "break" },

                new FeatureRow { Classification = "affiliation", Value = "University of Edinburgh" },
                new FeatureRow { Classification = "email", Value = "jcheney@inf.ed.ac.uk" },

                new FeatureRow { Classification = "other", Value = "break" },

                new FeatureRow { Classification = "author", Value = "Sam Lindley" },
                new FeatureRow { Classification = "affiliation", Value = "University of Strathclyde" },
                new FeatureRow { Classification = "email", Value = "sam.lindley@strath.ac.uk" },
            };

            var testSubject = new HeaderFactory();
            var model = testSubject.Create(featureRows);

            model.Authors.Should().HaveCount(2);

            model.Authors[0].Name.Should().Be("James Cheney");
            model.Authors[0].Affiliation.Should().Be("University of Edinburgh");
            model.Authors[0].Email.Should().Be("jcheney@inf.ed.ac.uk");

            model.Authors[1].Name.Should().Be("Sam Lindley");
            model.Authors[1].Affiliation.Should().Be("University of Strathclyde");
            model.Authors[1].Email.Should().Be("sam.lindley@strath.ac.uk");
        }

        [Fact]
        public void Keywords()
        {
            var featureRows = new[]
            {
                new FeatureRow { IsStart = true, Classification = "keyword", Value = "Keywords" },
                new FeatureRow { Classification = "keyword", Value = "lambda" },
                new FeatureRow { Classification = "keyword", Value = "calculus" },
                new FeatureRow { Classification = "keyword", Value = "," },
                new FeatureRow { Classification = "keyword", Value = "LINQ" },
                new FeatureRow { Classification = "keyword", Value = "," },
                new FeatureRow { Classification = "keyword", Value = "F#" },
                new FeatureRow { Classification = "keyword", Value = "," },
                new FeatureRow { Classification = "keyword", Value = "quotation" },
                new FeatureRow { Classification = "keyword", Value = "," },
                new FeatureRow { Classification = "keyword", Value = "anti" },
                new FeatureRow { Classification = "keyword", Value = "-" },
                new FeatureRow { Classification = "keyword", Value = "quotation" },
            };

            var testSubject = new HeaderFactory();
            var model = testSubject.Create(featureRows);

            model.Keywords.Should().HaveCount(5);
            model.Keywords[0].Should().Be("lambda calculus");
            model.Keywords[1].Should().Be("LINQ");
            model.Keywords[2].Should().Be("F#");
            model.Keywords[3].Should().Be("quotation");
            model.Keywords[4].Should().Be("anti-quotation");
        }
    }
}
