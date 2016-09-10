using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
