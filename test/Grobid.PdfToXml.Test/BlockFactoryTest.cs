using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using Xunit;

namespace Grobid.PdfToXml.Test
{

    public class BlockFactoryTest
    {
        [Fact]
        public void Test00()
        {
            var testInfoFactory = new PageBlockFactory();
            var pageBlocks = testInfoFactory.Create(Sample.Pdf.OpenEssenseLinq(), 1);
            var textBlocks = pageBlocks.First().TextBlocks;

            var index = 3;

            var textBlockA = textBlocks[index - 1];
            var textBlockB = textBlocks[index + 0];
            var textBlockC = textBlocks[index + 1];

            textBlockA.IsSameBlock(textBlockB).Should().BeTrue();
            textBlockB.IsSameBlock(textBlockC).Should().BeFalse();
        }

        [Fact]
        public void Test01()
        {
            var testInfoFactory = new PageBlockFactory();
            var pageBlocks = testInfoFactory.Create(Sample.Pdf.OpenEssenseLinq(), 1);
            var textBlocks = pageBlocks.First().TextBlocks;

            var xss = new List<List<TextBlock>>
            {
                new List<TextBlock>
                {
                    textBlocks[0]
                },
            };

            int index = 1;
            int offset = 0;

            while (index < textBlocks.Length)
            {
                for (; index < textBlocks.Length; index++)
                {
                    if (xss[offset].Last().IsSameBlock(textBlocks[index]))
                    {
                        xss[offset].Add(textBlocks[index]);
                    }
                    else
                    {
                        xss.Add(
                            new List<TextBlock>
                            {
                                textBlocks[index]
                            });
                        break;
                    }
                }

                offset++;
                index++;
            }
        }
    }

    public static class Extensions
    {
        // Same rules as pdftoxml.

        // Max difference in primary font sizes on two lines in the same
        // block.  Delta1 is used when examining new lines above and below the
        // current block.
        private const double maxBlockFontSizeDelta1 = 0.05;

        // Max distance between baselines of two lines within a block, as a
        // fraction of the font size.
        private const double maxLineSpacingDelta = 1.5;

        public static bool IsSameBlock(this TextBlock textBlockA, TextBlock textBlockB)
        {
            bool isA = textBlockB.Y + textBlockB.Height >= textBlockA.Y;
            bool isB = Math.Abs(textBlockB.TokenBlocks[0].FontSize - textBlockA.TokenBlocks[0].FontSize) <
                       (textBlockB.TokenBlocks[0].FontSize * Extensions.maxBlockFontSizeDelta1);
            bool isC = (textBlockB.Y - textBlockA.Y) < (textBlockA.TokenBlocks[0].FontSize * maxLineSpacingDelta);

            return (isA && isB && isC);
        }
    }
}
