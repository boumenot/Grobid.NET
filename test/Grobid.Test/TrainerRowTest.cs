using System;

using FluentAssertions;
using Xunit;

using Grobid.NET;

namespace Grobid.Test
{
    public class TrainerRowTest
    {
        [Fact]
        public void TrainerRowAcceptableValues()
        {
            var rows = new[]
            {
                "The the 1 1 0 1 0 LINESTART 0 1 <title> <title>",
                "The the 1 1 0 1 0 LINESTART 0 1 <title> I-<title>",
                "The the 1 1 0 1 0 LINESTART 0 1 I-<title> <title>",
                "The the 1 1 0 1 0 LINESTART 0 1 I-<title> I-<title>",
            };

            foreach (var row in rows)
            {
                var testSubject = TrainerRow.Parse(row);

                testSubject.Expected.Should().Be("title");
                testSubject.Obtained.Should().Be("title");
                testSubject.Value.Should().Be("The");
            }
        }

        [Fact]
        public void TrainerRowDifferentClassifications()
        {
            var testSubject = TrainerRow.Parse("The the 1 1 0 1 0 LINESTART 0 1 <expected> <obtained>");

            testSubject.Expected.Should().Be("expected");
            testSubject.Obtained.Should().Be("obtained");
            testSubject.Value.Should().Be("The");
        }

        [Fact]
        public void TrainerRowMalformedValues()
        {
            var rows = new[]
            {
                "The the 1 1 0 1 0 LINESTART 0 1 title",          // no expected value
                "The the 1 1 0 1 0 LINESTART 0 1 title>",         // no '<'
                "The the 1 1 0 1 0 LINESTART 0 1 <title>",        // no obtained value
                "The the 1 1 0 1 0 LINESTART 0 1 <title> <title", // incomplete obtained value
                "The the 1 1 0 1 0 LINESTART 0 1 >title<",        // '>' before '<'
                "The\tthe\t1\t1\t0\t1\t0\tLINESTART\t0\t1\t<title>\t<title>", // tab separator instead of spaces
                "<title>",                                        // no value
                "<title> <title>",                                // no value
            };

            foreach (var row in rows)
            {
                Action action = () => TrainerRow.Parse(row);
                action.ShouldThrow<ArgumentException>(row);
            }
        }
    }
}
