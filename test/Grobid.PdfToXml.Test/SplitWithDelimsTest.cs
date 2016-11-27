using System;
using System.Linq;

using FluentAssertions;
using Xunit;

namespace Grobid.PdfToXml.Test
{
    public class SplitWithDelimsTest
    {
        [Fact]
        public void Test01()
        {
            var xs = "The wizard quickly jinxed the gnomes before they vaporized".SplitWithDelims(Constants.FullPunctuation);
            xs.Should().ContainInOrder("The", " ", "wizard", " ", "quickly", " ", "jinxed", " ", "the", " ", "gnomes", " ", "before", " ", "they", " ", "vaporized");
        }

        [Fact]
        public void Test02()
        {
            var xs = "abc,def.ghi!.jkl;mno".SplitWithDelims(Constants.FullPunctuation);
            xs.Should().ContainInOrder(
                "abc",
                ",",
                "def",
                ".",
                "ghi",
                "!",
                "jkl",
                ";",
                "mno");
        }

        [Fact]
        public void Test03()
        {
            var xs = "".SplitWithDelims(Constants.FullPunctuation);
            xs.Should().ContainInOrder("");
        }

        [Fact]
        public void Test04()
        {
            var xs = "IsOneToken".SplitWithDelims(Constants.FullPunctuation);
            xs.Should().ContainInOrder("IsOneToken");
        }

        [Fact]
        public void Test05()
        {
            string s = null;
            Action test = () => s.SplitWithDelims(Constants.FullPunctuation).ToArray();
            test.ShouldThrow<NullReferenceException>();
        }
    }
}
