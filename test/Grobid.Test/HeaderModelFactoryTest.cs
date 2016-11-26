using System;
using System.Collections.Generic;

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
            var testSubject = new HeaderModelFactory();
            var model = testSubject.Create();

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
    }

    public class HeaderModelFactory
    {
        private readonly FeatureRowTextJoiner textJoiner = new FeatureRowTextJoiner();
        private readonly SentenceTextJoiner sentenceJoiner = new SentenceTextJoiner();

        public HeaderModel Create()
        {
            throw new NotImplementedException();
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
