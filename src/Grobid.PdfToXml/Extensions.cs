using System;
using System.Collections;
using System.Collections.Generic;

namespace Grobid.PdfToXml
{
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

        public static bool IsSameBlock(this TextBlock prevTextBlock, TextBlock currTextBlock)
        {
            bool isSimilarLineHeight = currTextBlock.Y + currTextBlock.Height >= prevTextBlock.Y;
            bool isSimilarFontSize = Math.Abs(currTextBlock.TokenBlocks[0].FontSize - prevTextBlock.TokenBlocks[0].FontSize) <
                                     (currTextBlock.TokenBlocks[0].FontSize * Extensions.maxBlockFontSizeDelta1);
            bool isMinimalSpacing = (currTextBlock.Y - prevTextBlock.Y) < (prevTextBlock.TokenBlocks[0].FontSize * Extensions.maxLineSpacingDelta);

            return (isSimilarLineHeight && isSimilarFontSize && isMinimalSpacing);
        }

        public static IEnumerable<string> SplitWithDelims(this string s, char[] delimiters)
        {
            int index, offset = 0;

            while ((index = s.IndexOfAny(delimiters, offset)) != -1)
            {
                int length = index - offset;
                if (length > 0)
                {
                    yield return s.Substring(offset, length);
                }

                yield return s.Substring(index, 1);
                offset = index + 1;
            }

            if (offset < s.Length)
            {
                yield return s.Substring(offset);
            }

            if (s == String.Empty)
            {
                yield return s;
            }
        }
    }
}