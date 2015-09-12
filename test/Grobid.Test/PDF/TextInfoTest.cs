using System;

using FluentAssertions;

using Grobid.NET;

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
    }
}
