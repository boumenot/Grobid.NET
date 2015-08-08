using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Grobid.NET;

namespace Grobid.Test
{
    public class LexiconFactoryTest
    {
        [Fact]
        public void CreateFromWords()
        {
            var testSubject = new LexiconFactory();
            var lines = new[] {
                "abc",
                "def",
            };

            var lexicon = testSubject.Create(lines.ToStream());
            lexicon.HasWord("abc").Should().BeTrue();
        }

        [Fact]
        public void Create()
        {
            var testSubject = new LexiconFactory();
            var firstNames = new[] { "Patrick", "Eugene" };
            var lastNames = new[] { "Star", "Krabs" };
            var words = new[] { "abc", "def" };

            var lexicon = testSubject.Create(
                firstNames.ToStream(),
                lastNames.ToStream(),
                words.ToStream());

            lexicon.IsFirstName("patrick").Should().BeTrue();
            lexicon.IsLastName("star").Should().BeTrue();
            lexicon.HasWord("def").Should().BeTrue();
        }

    }
}
