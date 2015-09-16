using FluentAssertions;
using Xunit;

namespace Grobid.PdfToXml.Test
{
    public class IndexGeneratorTest
    {
        [Fact]
        public void PageIndexShouldIncrementOnCall()
        {
            var testSubject = new IndexGenerator();
            testSubject.PageIndex.Should().Be("p1");
            testSubject.PageIndex.Should().Be("p2");
            testSubject.PageIndex.Should().Be("p3");
        }

        [Fact]
        public void TextIndexShouldIncrementOnCall()
        {
            var testSubject = new IndexGenerator();
            testSubject.PageIndex.Should().Be("p1");
            testSubject.TextIndex.Should().Be("p1_t1");
            testSubject.TextIndex.Should().Be("p1_t2");
        }

        [Fact]
        public void TextIndexShouldRollOnPageChange()
        {
            var testSubject = new IndexGenerator();
            testSubject.PageIndex.Should().Be("p1");
            testSubject.TextIndex.Should().Be("p1_t1");
            testSubject.TextIndex.Should().Be("p1_t2");
            testSubject.PageIndex.Should().Be("p2");
            testSubject.TextIndex.Should().Be("p2_t1");
            testSubject.TextIndex.Should().Be("p2_t2");
        }

        [Fact]
        public void TokenIndexShouldIncrementOnCall()
        {
            var testSubject = new IndexGenerator();
            testSubject.PageIndex.Should().Be("p1");
            testSubject.TokenIndex.Should().Be("p1_w1");
            testSubject.TokenIndex.Should().Be("p1_w2");
        }

        [Fact]
        public void TokenIndexShouldRollOnPageChange()
        {
            var testSubject = new IndexGenerator();
            testSubject.PageIndex.Should().Be("p1");
            testSubject.TokenIndex.Should().Be("p1_w1");
            testSubject.TokenIndex.Should().Be("p1_w2");
            testSubject.PageIndex.Should().Be("p2");
            testSubject.TokenIndex.Should().Be("p2_w1");
            testSubject.TokenIndex.Should().Be("p2_w2");
        }

        [Fact]
        public void TokenIndexDoesNotRollOnTextChange()
        {
            var testSubject = new IndexGenerator();
            testSubject.PageIndex.Should().Be("p1");
            testSubject.TextIndex.Should().Be("p1_t1");
            testSubject.TokenIndex.Should().Be("p1_w1");
            testSubject.TokenIndex.Should().Be("p1_w2");
            testSubject.TextIndex.Should().Be("p1_t2");
            testSubject.TokenIndex.Should().Be("p1_w3");
            testSubject.TokenIndex.Should().Be("p1_w4");
        }

        [Fact]
        public void SidIncrementOnEveryChange()
        {
            var testSubject = new IndexGenerator();
            testSubject.PageIndex.Should().Be("p1");
            testSubject.TextIndex.Should().Be("p1_t1");

            testSubject.SidIndex.Should().Be("p1_s3");
            testSubject.SidIndex.Should().Be("p1_s4");

            testSubject.PageIndex.Should().Be("p2");
            testSubject.TextIndex.Should().Be("p2_t1");

            testSubject.SidIndex.Should().Be("p2_s7");
            testSubject.SidIndex.Should().Be("p2_s8");
        }
    }
}
