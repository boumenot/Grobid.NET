using System;

using FluentAssertions;
using iTextSharp.text.pdf.parser;
using Xunit;

namespace Grobid.PdfToXml.Test
{
    public class TokenBlockTest
    {
        [Fact]
        public void CreateEmptyShouldReturnInstance()
        {
            var testSubject = TokenBlock.Empty;
            testSubject.Should().NotBeNull();
        }

        [Fact]
        public void CreateEmptyTextBlockIsEmpty()
        {
            var testSubject = TokenBlock.Empty;
            testSubject.IsEmpty.Should().BeTrue();
        }

        //[Fact]
        //public void CreateTextBlockIsEmptyShouldBeFalse()
        //{
        //    var testSubject = TokenBlock.Create(null, null, new Vector(0, 0, 0), new Vector(0, 0, 0));
        //    testSubject.IsEmpty.Should().BeFalse();
        //}

        //[Fact]
        //public void CreateTextBlockText()
        //{
        //    var testSubject = TokenBlock.Create("text", null, new Vector(0, 0, 0), new Vector(0, 0, 0));
        //    testSubject.Text.Should().Be("text");
        //}

        //[Fact]
        //public void CreateTextBlockLineSegment()
        //{
        //    var lineSegment = new LineSegment(
        //        new Vector(0, 1, 0),
        //        new Vector(2, 3, 0));

        //    var testSubject = TokenBlock.Create(null, lineSegment, new Vector(0, 0, 0), new Vector(0, 0, 0));
        //    testSubject.StartPoint.Should().Be(lineSegment.GetStartPoint());
        //    testSubject.EndPoint.Should().Be(lineSegment.GetEndPoint());
        //}
    }
}
