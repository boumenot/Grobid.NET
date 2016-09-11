using FluentAssertions;
using Xunit;

namespace Grobid.Test
{
    public class FeatureExtractorTest
    {
        [Fact]
        public void TestPrefix()
        {
            var testSubject = new FeatureExtractor(EmptyLexicon.Instance);
            testSubject.Prefix("Christopher", 1).Should().Be("C");
            testSubject.Prefix("Christopher", 2).Should().Be("Ch");
            testSubject.Prefix("Christopher", 3).Should().Be("Chr");

            testSubject.Prefix("abc", 4).Should().Be("abc");
            testSubject.Prefix("abc", 3).Should().Be("abc");
        }

        [Fact]
        public void TestSuffix()
        {
            var testSubject = new FeatureExtractor(EmptyLexicon.Instance);
            testSubject.Suffix("Christopher", 1).Should().Be("r");
            testSubject.Suffix("Christopher", 2).Should().Be("er");
            testSubject.Suffix("Christopher", 3).Should().Be("her");

            testSubject.Suffix("abc", 4).Should().Be("abc");
            testSubject.Suffix("abc", 3).Should().Be("abc");
        }

        [Fact]
        public void TestCapitalization()
        {
            var testSubject = new FeatureExtractor(EmptyLexicon.Instance);
            testSubject.Case("UPPER").Should().Be(Capitalization.ALLCAP);
            testSubject.Case("Capital").Should().Be(Capitalization.INITCAP);
            testSubject.Case("lower").Should().Be(Capitalization.NOCAPS);
            testSubject.Case("").Should().Be(Capitalization.NOCAPS);
        }

        [Fact]
        public void TestDigit()
        {
            var testSubject = new FeatureExtractor(EmptyLexicon.Instance);
            testSubject.Digit("").Should().Be(Digit.NODIGIT);
            testSubject.Digit("abc").Should().Be(Digit.NODIGIT);
            testSubject.Digit("abc123").Should().Be(Digit.CONTAINDIGIT);
            testSubject.Digit("123").Should().Be(Digit.ALLDIGIT);
        }

        [Fact]
        public void IsSingleCharTest()
        {
            var testSubject = new FeatureExtractor(EmptyLexicon.Instance);
            testSubject.IsSingleChar("").Should().BeFalse();
            testSubject.IsSingleChar("a").Should().BeTrue();
            testSubject.IsSingleChar("abc").Should().BeFalse();
        }

        [Fact]
        public void IsMonth()
        {
            var testSubject = new FeatureExtractor(EmptyLexicon.Instance);
            testSubject.IsMonth("January").Should().BeTrue();
            testSubject.IsMonth("February").Should().BeTrue();
            testSubject.IsMonth("March").Should().BeTrue();
            testSubject.IsMonth("April").Should().BeTrue();
            testSubject.IsMonth("May").Should().BeTrue();
            testSubject.IsMonth("June").Should().BeTrue();
            testSubject.IsMonth("July").Should().BeTrue();
            testSubject.IsMonth("August").Should().BeTrue();
            testSubject.IsMonth("September").Should().BeTrue();
            testSubject.IsMonth("October").Should().BeTrue();
            testSubject.IsMonth("November").Should().BeTrue();
            testSubject.IsMonth("December").Should().BeTrue();
            testSubject.IsMonth("Jan").Should().BeTrue();
            testSubject.IsMonth("Feb").Should().BeTrue();
            testSubject.IsMonth("Mar").Should().BeTrue();
            testSubject.IsMonth("Apr").Should().BeTrue();
            testSubject.IsMonth("Jun").Should().BeTrue();
            testSubject.IsMonth("Jul").Should().BeTrue();
            testSubject.IsMonth("Aug").Should().BeTrue();
            testSubject.IsMonth("Sep").Should().BeTrue();
            testSubject.IsMonth("Oct").Should().BeTrue();
            testSubject.IsMonth("Nov").Should().BeTrue();
            testSubject.IsMonth("Dec").Should().BeTrue();

            // case does not matter
            testSubject.IsMonth("january").Should().BeTrue();

            // negative
            testSubject.IsMonth("abc").Should().BeFalse();
        }

        [Fact]
        public void IsYear()
        {
            var testSubject = new FeatureExtractor(EmptyLexicon.Instance);
            testSubject.IsYear("0000").Should().BeFalse();
            testSubject.IsYear("1000").Should().BeTrue();
            testSubject.IsYear("2000").Should().BeTrue();
            testSubject.IsYear("2999").Should().BeTrue();
            testSubject.IsYear("3000").Should().BeFalse();

            testSubject.IsYear("").Should().BeFalse();
            testSubject.IsYear("abc").Should().BeFalse();
        }

        [Fact]
        public void IsEmailAddress()
        {
            var testSubject = new FeatureExtractor(EmptyLexicon.Instance);
            testSubject.IsEmailAddress("me@here.com").Should().BeTrue();
            testSubject.IsEmailAddress("me.you@here.com").Should().BeTrue();

            testSubject.IsEmailAddress("me@").Should().BeFalse();
            testSubject.IsEmailAddress("email").Should().BeFalse();
        }

        [Fact]
        public void TestHashHttp()
        {
            var testSubject = new FeatureExtractor(EmptyLexicon.Instance);
            testSubject.HasHttp("http").Should().BeTrue();
            testSubject.HasHttp("http://archive.org").Should().BeTrue();
            testSubject.HasHttp("https://archive.org").Should().BeTrue();
            testSubject.HasHttp("InTheMiddle_http_InTheMiddle").Should().BeTrue();

            testSubject.HasHttp("HTTP").Should().BeFalse();
        }

        [Fact]
        public void TestHasDash()
        {
            var testSubject = new FeatureExtractor(EmptyLexicon.Instance);
            testSubject.HasDash("-").Should().BeTrue();
            testSubject.HasDash("a-bc").Should().BeTrue();
            testSubject.HasDash("abc").Should().BeFalse();
        }

        [Fact]
        public void TestPunctuation()
        {
            var testSubject = new FeatureExtractor(EmptyLexicon.Instance);
            testSubject.Punctuation(":").Should().Be(Punctuation.PUNCT);
            testSubject.Punctuation(";").Should().Be(Punctuation.PUNCT);
            testSubject.Punctuation("?").Should().Be(Punctuation.PUNCT);

            testSubject.Punctuation(",").Should().Be(Punctuation.COMMA);
            testSubject.Punctuation(".").Should().Be(Punctuation.DOT);
            testSubject.Punctuation("-").Should().Be(Punctuation.HYPHEN);
            testSubject.Punctuation("(").Should().Be(Punctuation.OPENBRACKET);
            testSubject.Punctuation("[").Should().Be(Punctuation.OPENBRACKET);
            testSubject.Punctuation(")").Should().Be(Punctuation.ENDBRACKET);
            testSubject.Punctuation("]").Should().Be(Punctuation.ENDBRACKET);
            testSubject.Punctuation("\"").Should().Be(Punctuation.QUOTE);
            testSubject.Punctuation("'").Should().Be(Punctuation.QUOTE);
            testSubject.Punctuation("`").Should().Be(Punctuation.QUOTE);

            testSubject.Punctuation("a").Should().Be(Punctuation.NOPUNCT);
            testSubject.Punctuation("dog").Should().Be(Punctuation.NOPUNCT);
        }

        [Fact]
        public void TestIsDictionaryWord()
        {
            var testSubject = new FeatureExtractor(EmptyLexicon.Instance);
            testSubject.IsDictionaryWord("word").Should().BeTrue();
            testSubject.IsDictionaryWord("not-in-dictionary").Should().BeFalse();
        }

        [Fact]
        public void TestIsName()
        {
            var testSubject = new FeatureExtractor(EmptyLexicon.Instance);
            testSubject.IsForename("Christopher").Should().BeTrue();
            testSubject.IsForename("christopher").Should().BeTrue();
            testSubject.IsForename("Seth").Should().BeFalse();

            testSubject.IsSurname("Boumenot").Should().BeTrue();
            testSubject.IsSurname("boumenot").Should().BeTrue();
            testSubject.IsSurname("Smith").Should().BeFalse();

            testSubject.IsName("Christopher").Should().BeTrue();
            testSubject.IsName("christopher").Should().BeTrue();
            testSubject.IsName("Boumenot").Should().BeTrue();
            testSubject.IsName("boumenot").Should().BeTrue();
            testSubject.IsName("Smith").Should().BeFalse();
        }
    }
}
