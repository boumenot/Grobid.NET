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
            var testSubject = TextInfo.CreateEmpty();
            testSubject.Should().NotBeNull();
        }

        [Fact]
        public void CreateEmptyTextInfoBoundingRectangleShouldBeNull()
        {
            var testSubject = TextInfo.CreateEmpty();
            testSubject.BoundingRectangle.Should().BeNull();
        }

        [Fact]
        public void CreateEmptyTextInfoTextShouldBeNull()
        {
            var testSubject = TextInfo.CreateEmpty();
            testSubject.Text.Should().BeNull();
        }

        [Fact]
        public void CreateEmptyTextInfoStartPointShouldThrow()
        {
            var testSubject = TextInfo.CreateEmpty();
            Action test = () => { var x = testSubject.StartPoint; };
            test.ShouldThrow<NullReferenceException>();
        }

        [Fact]
        public void CreateEmptyTextInfoEndPointShouldThrow()
        {
            var testSubject = TextInfo.CreateEmpty();
            Action test = () => { var x = testSubject.EndPoint; };
            test.ShouldThrow<NullReferenceException>();
        }

        [Fact]
        public void CreateTextInfoBoundingRectangle()
        {
            var bottomLeft = new Vector(1.0f, 2.0f, 0.0f);
            var topRight = new Vector(3.0f, 4.0f, 0.0f);

            var testSubject = TextInfo.Create(null, null, bottomLeft, topRight);

            testSubject.BoundingRectangle.GetLeft(0).Should().Be(1.0f);
            testSubject.BoundingRectangle.GetBottom(0).Should().Be(2.0f);
            testSubject.BoundingRectangle.GetRight(0).Should().Be(3.0f);
            testSubject.BoundingRectangle.GetTop(0).Should().Be(4.0f);
        }

        [Fact]
        public void CreateTextInfoText()
        {
            var testSubject = TextInfo.Create("text", null, new Vector(0, 0, 0), new Vector(0, 0, 0));
            testSubject.Text.Should().Be("text");
        }

        [Fact]
        public void CreateTextInfoLineSegment()
        {
            var lineSegment = new LineSegment(
                new Vector(0, 1, 0),
                new Vector(2, 3, 0));

            var testSubject = TextInfo.Create(null, lineSegment, new Vector(0, 0, 0), new Vector(0, 0, 0));
            testSubject.StartPoint.Should().Be(lineSegment.GetStartPoint());
            testSubject.EndPoint.Should().Be(lineSegment.GetEndPoint());
        }
    }
}
