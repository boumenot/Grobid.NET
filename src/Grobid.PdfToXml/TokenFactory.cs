using System;

using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace Grobid.PdfToXml
{
    public class TokenFactory
    {
        private static readonly string Black = "#000000";

        private readonly float pageWidth;
        private readonly float pageHeight;

        public TokenFactory(float pageWidth, float pageHeight)
        {
            this.pageWidth = pageWidth;
            this.pageHeight = pageHeight;
        }

        public TokenBlock Create(TextRenderInfo renderInfo)
        {
            var fontDescriptor = renderInfo.GetFont().FontDictionary.GetAsDict(PdfName.FONTDESCRIPTOR);
            var boundingRectangle = this.GetBoundingRectangle(renderInfo);

            var fontSize = this.GetFontSize(boundingRectangle);
            var tokenBlockBase = this.GetBase(renderInfo);

            var yMin = tokenBlockBase - this.GetFontAscent(fontDescriptor, fontSize);
            var yMax = tokenBlockBase - this.GetFontDescent(fontDescriptor, fontSize);

            var tokenBlock = new TokenBlock
            {
                Angle = this.GetAngle(),
                Base = tokenBlockBase,
                BoundingRectangle = boundingRectangle,
                FontColor = this.GetFontColor(renderInfo),
                FontFlags = this.GetFontFlags(fontDescriptor),
                FontName = this.GetFontName(renderInfo),
                FontSize = fontSize,
                Height = yMax - yMin,
                IsEmpty = false,
                Rotation = this.GetRotation(),
                Text = renderInfo.GetText(),
                Width = this.GetWidth(renderInfo),
                X = this.GetX(renderInfo),
                Y = yMin,
            };

            return tokenBlock;
        }

        private float GetFontAscent(PdfDictionary fontDescriptor, float fontSize)
        {
            var ascent = fontDescriptor.GetAsNumber(PdfName.ASCENT).FloatValue / 1000;
            return ascent * fontSize;
        }

        private float GetFontDescent(PdfDictionary fontDescriptor, float fontSize)
        {
            var descent = fontDescriptor.GetAsNumber(PdfName.DESCENT).FloatValue / 1000;
            return descent * fontSize;
        }

        private Rectangle GetBoundingRectangle(TextRenderInfo renderInfo)
        {
            var rectangle = new Rectangle(
                renderInfo.GetDescentLine().GetStartPoint().X(),
                renderInfo.GetDescentLine().GetStartPoint().Y(),
                renderInfo.GetAscentLine().GetEndPoint().X(),
                renderInfo.GetAscentLine().GetEndPoint().Y());

            return rectangle;
        }

        private FontFlags GetFontFlags(PdfDictionary fontDescriptor)
        {
            var flags = fontDescriptor.GetAsNumber(PdfName.FLAGS);
            return (FontFlags)(flags == null ? 0 : flags.IntValue);
        }

        private int GetAngle() { return 0; }

        private float GetBase(TextRenderInfo renderInfo)
        {
            return this.pageHeight - renderInfo.GetBaseline().GetBoundingRectange().Y;
        }

        private string GetFontColor(TextRenderInfo renderInfo)
        {
            var strokeColor = renderInfo.GetStrokeColor();
            return strokeColor == null
                       ? TokenFactory.Black
                       : this.StrokeColorToHexColor(strokeColor);
        }

        private string StrokeColorToHexColor(BaseColor strokeColor)
        {
            return String.Format("#{0:x2}{1:x2}{2:x2}",
                strokeColor.R,
                strokeColor.B,
                strokeColor.G);
        }

        private FontName GetFontName(TextRenderInfo renderInfo)
        {
            return FontName.Parse(
                renderInfo.GetFont().PostscriptFontName);
        }

        private float GetFontSize(Rectangle boundingRectangle)
        {
            return boundingRectangle.Height;
        }

        private int GetRotation() { return 0; }

        private float GetWidth(TextRenderInfo renderInfo)
        {
            return renderInfo.GetBaseline().GetEndPoint().X() - this.GetX(renderInfo);
        }

        private float GetX(TextRenderInfo renderInfo)
        {
            return renderInfo.GetBaseline().GetStartPoint().X();
        }
    }
}