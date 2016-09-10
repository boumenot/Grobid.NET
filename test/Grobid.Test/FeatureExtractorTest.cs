using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;
using Xunit;

namespace Grobid.Test
{
    public class FeatureExtractorTest
    {
        [Fact]
        public void TestPrefix()
        {
            var testSubject = new FeatureExtractor();
            testSubject.Prefix("Christopher", 1).Should().Be("C");
            testSubject.Prefix("Christopher", 2).Should().Be("Ch");
            testSubject.Prefix("Christopher", 3).Should().Be("Chr");

            testSubject.Prefix("abc", 4).Should().Be("abc");
            testSubject.Prefix("abc", 3).Should().Be("abc");
        }

        [Fact]
        public void TestSuffix()
        {
            var testSubject = new FeatureExtractor();
            testSubject.Suffix("Christopher", 1).Should().Be("r");
            testSubject.Suffix("Christopher", 2).Should().Be("er");
            testSubject.Suffix("Christopher", 3).Should().Be("her");

            testSubject.Suffix("abc", 4).Should().Be("abc");
            testSubject.Suffix("abc", 3).Should().Be("abc");
        }

        [Fact]
        public void TestCapitalization()
        {
            var testSubject = new FeatureExtractor();
            testSubject.Case("UPPER").Should().Be(Capitalization.ALLCAP);
            testSubject.Case("Capital").Should().Be(Capitalization.INITCAP);
            testSubject.Case("lower").Should().Be(Capitalization.NOCAPS);
            testSubject.Case("").Should().Be(Capitalization.NOCAPS);
        }

        [Fact]
        public void TestDigit()
        {
            var testSubject = new FeatureExtractor();
            testSubject.Digit("").Should().Be(Digit.NODIGIT);
            testSubject.Digit("abc").Should().Be(Digit.NODIGIT);
            testSubject.Digit("abc123").Should().Be(Digit.CONTAINDIGIT);
            testSubject.Digit("123").Should().Be(Digit.ALLDIGIT);
        }

        [Fact]
        public void IsSingleCharTest()
        {
            var testSubject = new FeatureExtractor();
            testSubject.IsSingleChar("").Should().BeFalse();
            testSubject.IsSingleChar("a").Should().BeTrue();
            testSubject.IsSingleChar("abc").Should().BeFalse();
        }
    }

    public enum Capitalization
    {
        INITCAP,
        NOCAPS,
        ALLCAP,
    }

    public enum Digit
    {
        ALLDIGIT,
        CONTAINDIGIT,
        NODIGIT
    }

    public class FeatureExtractor
    {
        public string Prefix(string s, int length)
        {
            return s.Substring(0, Math.Min(s.Length, length));
        }

        public string Suffix(string s, int length)
        {
            int len = Math.Min(s.Length, length);
            int offset = s.Length - len;

            return s.Substring(offset, len);
        }

        public Capitalization Case(string s)
        {
            if (s.Length == 0)
            {
                return Capitalization.NOCAPS;
            }

            var cap = Capitalization.NOCAPS;
            if (Char.IsUpper(s[0]))
            {
                cap = Capitalization.INITCAP;
            }

            if (s.All(Char.IsUpper))
            {
                cap = Capitalization.ALLCAP;
            }

            return cap;
        }

        public Digit Digit(string s)
        {
            var digitCount = s.Count(Char.IsDigit);
            return digitCount == 0
                       ? Test.Digit.NODIGIT
                       : digitCount == s.Length
                           ? Test.Digit.ALLDIGIT
                           : Test.Digit.CONTAINDIGIT;
        }

        public bool IsSingleChar(string s)
        {
            return s.Length == 1;
        }
    }
}
