using System;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Grobid.PdfToXml
{
    public class TokenBlockFactory
    {
        private static readonly string Black = "#000000";

        private readonly float pageWidth;
        private readonly float pageHeight;

        public TokenBlockFactory(float pageWidth, float pageHeight)
        {
            this.pageWidth = pageWidth;
            this.pageHeight = pageHeight;
        }

        public TokenBlock Create(ITextRenderInfo renderInfo)
        {
            var boundingRectangle = this.GetBoundingRectangle(renderInfo);

            var fontSize = this.GetFontSize(boundingRectangle);
            var tokenBlockBase = this.GetBase(renderInfo);

            var yMin = tokenBlockBase - renderInfo.GetFontAscent() * fontSize;
            var yMax = tokenBlockBase - renderInfo.GetFontDescent() * fontSize;

            var tokenBlock = new TokenBlock
            {
                Angle = this.GetAngle(),
                Base = tokenBlockBase,
                BoundingRectangle = boundingRectangle,
                FontColor = this.GetFontColor(renderInfo),
                FontFlags = (FontFlags)renderInfo.GetFontFlags(),
                FontName = this.GetFontName(renderInfo),
                FontSize = fontSize,
                Height = yMax - yMin,
                IsEmpty = false,
                Rotation = this.GetRotation(),
                Text = renderInfo.GetText().Normalize(),
                Width = this.GetWidth(renderInfo),
                X = this.GetX(renderInfo),
                Y = yMin,
            };

            return tokenBlock;
        }

        private Rectangle GetBoundingRectangle(ITextRenderInfo renderInfo)
        {
            var rectangle = new Rectangle(
                renderInfo.GetDescentLine().GetStartPoint().X(),
                renderInfo.GetDescentLine().GetStartPoint().Y(),
                renderInfo.GetAscentLine().GetEndPoint().X(),
                renderInfo.GetAscentLine().GetEndPoint().Y());

            return rectangle;
        }

        private int GetAngle() { return 0; }

        private float GetBase(ITextRenderInfo renderInfo)
        {
            return this.pageHeight - renderInfo.GetBaseline().GetBoundingRectange().Y;
        }

        private string GetFontColor(ITextRenderInfo renderInfo)
        {
            var strokeColor = renderInfo.GetStrokeColor();
            return strokeColor == null
                       ? TokenBlockFactory.Black
                       : this.StrokeColorToHexColor(strokeColor);
        }

        private string StrokeColorToHexColor(BaseColor strokeColor)
        {
            return String.Format("#{0:x2}{1:x2}{2:x2}",
                strokeColor.R,
                strokeColor.B,
                strokeColor.G);
        }

        private FontName GetFontName(ITextRenderInfo renderInfo)
        {
            return FontName.Parse(renderInfo.GetPostscriptFontName());
        }

        private float GetFontSize(Rectangle boundingRectangle)
        {
            return boundingRectangle.Height;
        }

        private int GetRotation() { return 0; }

        private float GetWidth(ITextRenderInfo renderInfo)
        {
            return renderInfo.GetBaseline().GetEndPoint().X() - this.GetX(renderInfo);
        }

        private float GetX(ITextRenderInfo renderInfo)
        {
            return renderInfo.GetBaseline().GetStartPoint().X();
        }
    }
}