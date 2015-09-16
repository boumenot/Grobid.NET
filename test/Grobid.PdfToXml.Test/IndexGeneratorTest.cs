using System;

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
            testSubject.TextIndex.Should().Be("p1_t3");
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
    }

    public class IndexGenerator
    {
        private int pageIndex;
        private int textIndex;

        public string PageIndex
        {
            get
            {
                this.pageIndex++;
                this.textIndex = 0;
                return String.Format("p{0}", this.pageIndex);
            }
        }

        public string TextIndex
        {
            get
            {
                this.textIndex++;
                return String.Format("p{0}_t{1}", this.pageIndex, this.textIndex);
            }
        }
    }
}
