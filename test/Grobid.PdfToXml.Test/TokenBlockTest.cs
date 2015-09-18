using System;

using FluentAssertions;

using iTextSharp.text;
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

        [Fact]
        public void MergeShouldElongateRectangle()
        {
            var boundingRectangle1 = new Rectangle(0, 1, 1, 2);
            var boundingRectangle2 = new Rectangle(1, 1, 2, 2);
            var boundingRectangle3 = new Rectangle(2, 1, 3, 2);

            var tokenBlocks = new[]
            {
                new TokenBlock { Base = 1, Text = "Lan", BoundingRectangle = boundingRectangle1},
                new TokenBlock { Base = 1, Text = "gua", BoundingRectangle = boundingRectangle2},
                new TokenBlock { Base = 1, Text = "ge",  BoundingRectangle = boundingRectangle3 },
            };

            var testSubject = TokenBlock.Merge(tokenBlocks);
            testSubject.Text.Should().Be("Language");

            testSubject.BoundingRectangle.Left.Should().Be(0);
            testSubject.BoundingRectangle.Bottom.Should().Be(1);
            testSubject.BoundingRectangle.Top.Should().Be(2);
            testSubject.BoundingRectangle.Right.Should().Be(3);
        }

        [Fact]
        public void MergeShouldIncreaseWidth()
        {
            var boundingRectangle1 = new Rectangle(0, 1, 1, 2);
            var boundingRectangle2 = new Rectangle(1, 1, 6, 2);

            var tokenBlocks = new[]
            {
                new TokenBlock { Base = 1, Text = "ob",    BoundingRectangle = boundingRectangle1, Width = 1.3f },
                new TokenBlock { Base = 1, Text = "vious", BoundingRectangle = boundingRectangle2, Width = 4.9f },
            };

            var testSubject = TokenBlock.Merge(tokenBlocks);
            testSubject.Text.Should().Be("obvious");
            testSubject.BoundingRectangle.Width.Should().Be(6);
            testSubject.Width.Should().Be(6);
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
