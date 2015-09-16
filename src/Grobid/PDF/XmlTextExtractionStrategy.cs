// The code in this file is based on code from the iTextSharp project.  It is
// based on code of the 'SimpleTextExtractionStrategy.cs'.  This file is 
// licensed per the agreement below.
/*
 * $Id$
 *
 * This file is part of the iText project.
 * Copyright(c) 1998-2014 iText Group NV
 * Authors: Kevin Day, Bruno Lowagie, Paulo Soares, et al.
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License version 3
 * as published by the Free Software Foundation with the addition of the
 * following permission added to Section 15 as permitted in Section 7(a):
 * FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
 * ITEXT GROUP.ITEXT GROUP DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
 * OF THIRD PARTY RIGHTS
 *
 * This program is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
 * or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU Affero General Public License for more details.
 * You should have received a copy of the GNU Affero General Public License
 * along with this program; if not, see http://www.gnu.org/licenses or write to
 * the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
 * Boston, MA, 02110-1301 USA, or download the license from the following URL:
 * http://itextpdf.com/terms-of-use/
 *
 * The interactive user interfaces in modified source and object code versions
 * of this program must display Appropriate Legal Notices, as required under
 * Section 5 of the GNU Affero General Public License.
 *
 * In accordance with Section 7(b) of the GNU Affero General Public License,
 * a covered work must retain the producer line in every PDF that is created
 * or manipulated using iText.
 *
 * You can be released from the requirements of the license by purchasing
 * a commercial license.Buying such a license is mandatory as soon as you
 * develop commercial activities involving the iText software without
 * disclosing the source code of your own applications.
 * These activities include: offering paid services to customers as an ASP,
 * serving PDFs on the fly in a web application, shipping iText with a closed
 * source product.
 *
 * For more information, please contact iText Software Corp.at this
 * address: sales @itextpdf.com
 */

using System;
using System.Collections.Generic;
using System.Linq;

using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace Grobid.NET
{
    public class XmlTextExtractionStrategy : ITextExtractionStrategy
    {
        private Vector lastStart;
        private Vector lastEnd;
        private readonly List<TokenBlock> tokenBlocks;
        private readonly float pageWidth;
        private readonly float pageHeight;

        public XmlTextExtractionStrategy(List<TokenBlock> tokenBlocks, float pageWidth, float pageHeight)
        {
            this.tokenBlocks = tokenBlocks;
            this.pageWidth = pageWidth;
            this.pageHeight = pageHeight;
        }

        public void BeginTextBlock() {}

        public void RenderText(TextRenderInfo renderInfo)
        {
            bool firstRender = this.tokenBlocks.Count == 0;
            bool hardReturn = false;

            LineSegment segment = renderInfo.GetBaseline();
            Vector start = segment.GetStartPoint();
            Vector end = segment.GetEndPoint();

            Vector x1 = lastStart;

            if (!firstRender)
            {
                Vector x0 = start;
                Vector x2 = lastEnd;

                // see http://mathworld.wolfram.com/Point-LineDistance2-Dimensional.html
                float dist = (x2.Subtract(x1)).Cross((x1.Subtract(x0))).LengthSquared / x2.Subtract(x1).LengthSquared;

                float sameLineThreshold = 1f; // we should probably base this on the current font metrics, but 1 pt seems to be sufficient for the time being
                if (dist > sameLineThreshold)
                    hardReturn = true;

                // Note:  Technically, we should check both the start and end positions, in case the angle of the text changed without any displacement
                // but this sort of thing probably doesn't happen much in reality, so we'll leave it alone for now
            }

            if (hardReturn)
            {
                this.tokenBlocks.Last().Text += '\n';
            }
            else if (!firstRender)
            {
                if (!this.tokenBlocks.Last().Text.EndsWith(" ") && renderInfo.GetText().Length > 0 && renderInfo.GetText()[0] != ' ')
                {
                    // we only insert a blank space if the trailing character of the previous string wasn't a space, and the leading character of the current string isn't a space
                    float spacing = lastEnd.Subtract(start).Length;
                    if (spacing > renderInfo.GetSingleSpaceWidth() / 2f)
                    {
                        this.AppendChunk();
                    }
                }
            }

            this.AppendChunk(renderInfo, segment);

            lastStart = start;
            lastEnd = end;
        }

        public void EndTextBlock() {}

        public void RenderImage(ImageRenderInfo renderInfo) {}

        public string GetResultantText()
        {
            return string.Empty;
        }

        private void AppendChunk(TextRenderInfo renderInfo, LineSegment lineSegment)
        {
            var tokenBlock = TokenBlock.Create(
                renderInfo.GetText(),
                lineSegment,
                renderInfo.GetDescentLine().GetStartPoint(),
                renderInfo.GetAscentLine().GetEndPoint());

            tokenBlock.FontName = FontName.Parse(renderInfo.GetFont().PostscriptFontName);
            tokenBlock.FontColor = renderInfo.GetStrokeColor() == null ? "#000000" : this.GetFontColor(renderInfo.GetStrokeColor());
            tokenBlock.Flags = this.GetFlags(renderInfo.GetFont());
            tokenBlock.Base = this.pageHeight - lineSegment.GetBoundingRectange().Y;

            var fontDescriptor = renderInfo.GetFont().FontDictionary.GetAsDict(PdfName.FONTDESCRIPTOR);
            var ascent = (fontDescriptor.GetAsNumber(PdfName.ASCENT).FloatValue / 1000) * tokenBlock.FontSize;
            var descent = (fontDescriptor.GetAsNumber(PdfName.DESCENT).FloatValue / 1000) * tokenBlock.FontSize;

            var yMin = tokenBlock.Base - ascent;
            var yMax = tokenBlock.Base - descent;

            tokenBlock.Height = yMax - yMin;
            tokenBlock.Y = yMin;

            this.tokenBlocks.Add(tokenBlock);
        }

        private FontFlags GetFlags(DocumentFont font)
        {
            var flags = font.FontDictionary?.GetAsDict(PdfName.FONTDESCRIPTOR)?.GetAsNumber(PdfName.FLAGS)?.IntValue;
            return (FontFlags)(flags ?? 0);
        }

        private string GetFontColor(BaseColor getStrokeColor)
        {
            return String.Format(
                "#{0:x2}{1:x2}{2:x2}",
                getStrokeColor.R,
                getStrokeColor.B,
                getStrokeColor.G);
        }

        private void AppendChunk()
        {
            this.tokenBlocks.Add(TokenBlock.CreateEmpty());
        }
    }
}
