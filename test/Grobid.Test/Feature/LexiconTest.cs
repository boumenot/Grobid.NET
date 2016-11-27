using System.Collections.Generic;

using FluentAssertions;

using Grobid.NET.Feature;

using Xunit;

namespace Grobid.Test.Feature
{
    public class LexiconTest
    {
        [Fact]
        public void IsFirstName()
        {
            var hashSet = new HashSet<string>(new[] { "abc", "def" });
            var testSubject = new Lexicon(hashSet, null, null);

            testSubject.IsFirstName("abc").Should().BeTrue();
            testSubject.IsFirstName("cba").Should().BeFalse();
        }

        [Fact]
        public void IsLastName()
        {
            var hashSet = new HashSet<string>(new[] { "abc", "def" });
            var testSubject = new Lexicon(null, hashSet, null);

            testSubject.IsLastName("abc").Should().BeTrue();
            testSubject.IsLastName("cba").Should().BeFalse();
        }

        [Fact]
        public void HasWord()
        {
            var hashSet = new HashSet<string>(new[] { "abc", "def" });
            var testSubject = new Lexicon(null, null, hashSet);

            testSubject.HasWord("abc").Should().BeTrue();
            testSubject.HasWord("cba").Should().BeFalse();
        }

        [Fact]
        public void HasWordRemovesEndingPunctuation()
        {
            var hashSet = new HashSet<string>(new[] { "abc", "def" });
            var testSubject = new Lexicon(null, null, hashSet);

            testSubject.HasWord("abc.").Should().BeTrue();
            testSubject.HasWord("abc,").Should().BeTrue();
            testSubject.HasWord("abc:").Should().BeTrue();
            testSubject.HasWord("abc;").Should().BeTrue();
        }

        [Fact]
        public void HasWordChecksBothTermsWhenHyphenated()
        {
            var hashSet = new HashSet<string>(new[] { "abc", "def" });
            var testSubject = new Lexicon(null, null, hashSet);

            testSubject.HasWord("abc-abc").Should().BeTrue();
            testSubject.HasWord("abc-def").Should().BeTrue();
            testSubject.HasWord("abc-").Should().BeFalse();
        }

        [Fact]
        public void HasWordChecksBothTermsWhenSpaceSeparated()
        {
            var hashSet = new HashSet<string>(new[] { "abc", "def" });
            var testSubject = new Lexicon(null, null, hashSet);

            testSubject.HasWord("abc abc").Should().BeTrue();
            testSubject.HasWord("abc def").Should().BeTrue();
            testSubject.HasWord("abc ").Should().BeFalse();
        }
    }
}
