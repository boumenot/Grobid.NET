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
            var testSubject = TokenBlock.CreateEmpty();
            AssertionExtensions.Should((object)testSubject).NotBeNull();
        }

        [Fact]
        public void CreateEmptyTextBlockBoundingRectangleShouldBeNull()
        {
            var testSubject = TokenBlock.CreateEmpty();
            AssertionExtensions.Should((object)testSubject.BoundingRectangle).BeNull();
        }

        [Fact]
        public void CreateEmptyTextBlockIsEmpty()
        {
            var testSubject = TokenBlock.CreateEmpty();
            AssertionExtensions.Should((bool)testSubject.IsEmpty).BeTrue();
        }

        [Fact]
        public void CreateEmptyTextBlockTextShouldBeNull()
        {
            var testSubject = TokenBlock.CreateEmpty();
            AssertionExtensions.Should((string)testSubject.Text).BeNull();
        }

        [Fact]
        public void CreateEmptyTextBlockStartPointShouldThrow()
        {
            var testSubject = TokenBlock.CreateEmpty();
            Action test = () => { var x = testSubject.StartPoint; };
            test.ShouldThrow<NullReferenceException>();
        }

        [Fact]
        public void CreateEmptyTextBlockEndPointShouldThrow()
        {
            var testSubject = TokenBlock.CreateEmpty();
            Action test = () => { var x = testSubject.EndPoint; };
            test.ShouldThrow<NullReferenceException>();
        }

        [Fact]
        public void CreateTextBlockBoundingRectangle()
        {
            var bottomLeft = new Vector(1.0f, 2.0f, 0.0f);
            var topRight = new Vector(3.0f, 4.0f, 0.0f);

            var testSubject = TokenBlock.Create(null, null, bottomLeft, topRight);

            AssertionExtensions.Should((float)testSubject.BoundingRectangle.GetLeft(0)).Be(1.0f);
            AssertionExtensions.Should((float)testSubject.BoundingRectangle.GetBottom(0)).Be(2.0f);
            AssertionExtensions.Should((float)testSubject.BoundingRectangle.GetRight(0)).Be(3.0f);
            AssertionExtensions.Should((float)testSubject.BoundingRectangle.GetTop(0)).Be(4.0f);
        }

        [Fact]
        public void CreateTextBlockIsEmptyShouldBeFalse()
        {
            var testSubject = TokenBlock.Create(null, null, new Vector(0, 0, 0), new Vector(0, 0, 0));
            AssertionExtensions.Should((bool)testSubject.IsEmpty).BeFalse();
        }

        [Fact]
        public void CreateTextBlockText()
        {
            var testSubject = TokenBlock.Create("text", null, new Vector(0, 0, 0), new Vector(0, 0, 0));
            AssertionExtensions.Should((string)testSubject.Text).Be("text");
        }

        [Fact]
        public void CreateTextBlockLineSegment()
        {
            var lineSegment = new LineSegment(
                new Vector(0, 1, 0),
                new Vector(2, 3, 0));

            var testSubject = TokenBlock.Create(null, lineSegment, new Vector(0, 0, 0), new Vector(0, 0, 0));
            AssertionExtensions.Should((object)testSubject.StartPoint).Be(lineSegment.GetStartPoint());
            AssertionExtensions.Should((object)testSubject.EndPoint).Be(lineSegment.GetEndPoint());
        }
    }
}
