using System;

using FluentAssertions;
using Grobid.NET;
using iTextSharp.text.pdf.parser;
using Xunit;

namespace Grobid.Test.PDF
{
    public class TextInfoTest
    {
        [Fact]
        public void CreateEmptyShouldReturnInstance()
        {
            var testSubject = TokenBlock.CreateEmpty();
            testSubject.Should().NotBeNull();
        }

        [Fact]
        public void CreateEmptyTextInfoBoundingRectangleShouldBeNull()
        {
            var testSubject = TokenBlock.CreateEmpty();
            testSubject.BoundingRectangle.Should().BeNull();
        }

        [Fact]
        public void CreateEmptyTextInfoIsEmpty()
        {
            var testSubject = TokenBlock.CreateEmpty();
            testSubject.IsEmpty.Should().BeTrue();
        }

        [Fact]
        public void CreateEmptyTextInfoTextShouldBeNull()
        {
            var testSubject = TokenBlock.CreateEmpty();
            testSubject.Text.Should().BeNull();
        }

        [Fact]
        public void CreateEmptyTextInfoStartPointShouldThrow()
        {
            var testSubject = TokenBlock.CreateEmpty();
            Action test = () => { var x = testSubject.StartPoint; };
            test.ShouldThrow<NullReferenceException>();
        }

        [Fact]
        public void CreateEmptyTextInfoEndPointShouldThrow()
        {
            var testSubject = TokenBlock.CreateEmpty();
            Action test = () => { var x = testSubject.EndPoint; };
            test.ShouldThrow<NullReferenceException>();
        }

        [Fact]
        public void CreateTextInfoBoundingRectangle()
        {
            var bottomLeft = new Vector(1.0f, 2.0f, 0.0f);
            var topRight = new Vector(3.0f, 4.0f, 0.0f);

            var testSubject = TokenBlock.Create(null, null, bottomLeft, topRight);

            testSubject.BoundingRectangle.GetLeft(0).Should().Be(1.0f);
            testSubject.BoundingRectangle.GetBottom(0).Should().Be(2.0f);
            testSubject.BoundingRectangle.GetRight(0).Should().Be(3.0f);
            testSubject.BoundingRectangle.GetTop(0).Should().Be(4.0f);
        }

        [Fact]
        public void CreateTextInfoIsEmptyShouldBeFalse()
        {
            var testSubject = TokenBlock.Create(null, null, new Vector(0, 0, 0), new Vector(0, 0, 0));
            testSubject.IsEmpty.Should().BeFalse();
        }

        [Fact]
        public void CreateTextInfoText()
        {
            var testSubject = TokenBlock.Create("text", null, new Vector(0, 0, 0), new Vector(0, 0, 0));
            testSubject.Text.Should().Be("text");
        }

        [Fact]
        public void CreateTextInfoLineSegment()
        {
            var lineSegment = new LineSegment(
                new Vector(0, 1, 0),
                new Vector(2, 3, 0));

            var testSubject = TokenBlock.Create(null, lineSegment, new Vector(0, 0, 0), new Vector(0, 0, 0));
            testSubject.StartPoint.Should().Be(lineSegment.GetStartPoint());
            testSubject.EndPoint.Should().Be(lineSegment.GetEndPoint());
        }
    }
}
