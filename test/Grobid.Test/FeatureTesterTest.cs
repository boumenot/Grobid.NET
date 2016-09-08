using FluentAssertions;
using Xunit;

using Grobid.NET;

namespace Grobid.Test
{
    public class FeatureTesterTest
    {
        [Fact]
        public void TestAllCapital()
        {
            var testSubject = new FeatureTesterImpl(EmptyLexicon.Instance);
            testSubject.test_all_capital("ALLCAPS").Should().BeTrue();
            testSubject.test_all_capital("ALL_CAPS").Should().BeTrue();

            testSubject.test_all_capital("ALL123CAPS").Should().BeTrue();
            testSubject.test_all_capital("AllCaps").Should().BeFalse();
            testSubject.test_all_capital("allcaps").Should().BeFalse();
        }

        [Fact]
        public void TestDigit()
        {
            var testSubject = new FeatureTesterImpl(EmptyLexicon.Instance);
            testSubject.test_digit("123").Should().BeTrue();
            testSubject.test_digit("_1").Should().BeTrue();
            testSubject.test_digit("a1").Should().BeTrue();
            testSubject.test_digit("A1").Should().BeTrue();

            testSubject.test_digit("abc").Should().BeFalse();
            testSubject.test_digit("ABC").Should().BeFalse();
            testSubject.test_digit("_:'").Should().BeFalse();
        }

        [Fact]
        public void TestHttp()
        {
            var testSubject = new FeatureTesterImpl(EmptyLexicon.Instance);
            testSubject.test_http("http").Should().BeTrue();
            testSubject.test_http("http://archive.org").Should().BeTrue();
            testSubject.test_http("https://archive.org").Should().BeTrue();
            testSubject.test_http("InTheMiddle_http_InTheMiddle").Should().BeTrue();

            testSubject.test_http("HTTP").Should().BeFalse();
        }

        [Fact]
        public void TestMonthFullyQualifiedMonthName()
        {
            var fullyQualifiedMonths = new[] {
                "January",
                "February",
                "March",
                "April",
                "May",
                "June",
                "July",
                "August",
                "September",
                "October",
                "November",
                "December",
            };

            var testSubject = new FeatureTesterImpl(EmptyLexicon.Instance);

            foreach (var month in fullyQualifiedMonths)
            {
                testSubject.test_month(month).Should().BeTrue();
                testSubject.test_month(month.ToLower()).Should().BeTrue();
                testSubject.test_month(month.ToUpper()).Should().BeTrue();
            }

            testSubject.test_month("Monday").Should().BeFalse();
            testSubject.test_month("Tomorrow").Should().BeFalse();
        }

        [Fact]
        public void TestMonthAbbreviatedMonthName()
        {
            var abbreviatedMonths = new[] {
                "Jan",
                "Feb",
                "March",
                "Apr",
                "Jun",
                "Jul",
                "Aug",
                "Sep",
                "Oct",
                "Nov",
                "Dec",
            };

            var testSubject = new FeatureTesterImpl(EmptyLexicon.Instance);

            foreach (var month in abbreviatedMonths)
            {
                testSubject.test_month(month).Should().BeTrue();
                testSubject.test_month(month.ToLower()).Should().BeTrue();
                testSubject.test_month(month.ToUpper()).Should().BeTrue();
            }
        }


        [Fact]
        public void TestNames()
        {
            var testSubject = new FeatureTesterImpl(EmptyLexicon.Instance);
            testSubject.test_names("name");
        }

        [Fact]
        public void TestCommon()
        {
            var testSubject = new FeatureTesterImpl(EmptyLexicon.Instance);
            testSubject.test_common("name");
        }

        [Fact]
        public void TestPunct()
        {
            var testSubject = new FeatureTesterImpl(EmptyLexicon.Instance);
            testSubject.test_punct(",").Should().BeTrue();
            testSubject.test_punct(".").Should().BeTrue();
            testSubject.test_punct(":").Should().BeTrue();
            testSubject.test_punct(";").Should().BeTrue();
            testSubject.test_punct("?").Should().BeTrue();

            testSubject.test_punct(",.:;?").Should().BeTrue();

            testSubject.test_punct("a").Should().BeFalse();
            testSubject.test_punct("a;").Should().BeFalse();
        }

        [Fact]
        public void TestEmail()
        {
            var testSubject = new FeatureTesterImpl(EmptyLexicon.Instance);
            testSubject.test_email("me@here.com").Should().BeTrue();
            testSubject.test_email("me.you@here.com").Should().BeTrue();

            testSubject.test_email("me@").Should().BeFalse();
            testSubject.test_email("email").Should().BeFalse();
        }

        [Fact]
        public void TestNumber()
        {
            var testSubject = new FeatureTesterImpl(EmptyLexicon.Instance);
            testSubject.test_number("1").Should().BeTrue();
            testSubject.test_number("123").Should().BeTrue();

            testSubject.test_number("1,234").Should().BeFalse();
            testSubject.test_number("hello").Should().BeFalse();
        }

        [Fact]
        public void TestYear()
        {
            var testSubject = new FeatureTesterImpl(EmptyLexicon.Instance);
            testSubject.test_year("1000").Should().BeTrue();
            testSubject.test_year("2999").Should().BeTrue();

            testSubject.test_year("999").Should().BeFalse();
            testSubject.test_year("3000").Should().BeFalse();
            testSubject.test_year("horse").Should().BeFalse();
        }
    }
}
