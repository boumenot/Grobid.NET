using System;

using iTextSharp.text;
using iTextSharp.text.pdf.parser;

namespace Grobid.NET
{
    public class TokenBlock
    {
        private LineSegment Baseline { get; set; }

        public string Text { get; set; }

        public bool IsEmpty { get { return this.BoundingRectangle == null; } }

        public Vector StartPoint
        {
            get { return this.Baseline.GetStartPoint(); }
        }

        public Vector EndPoint
        {
            get { return this.Baseline.GetEndPoint(); }
        }

        public Rectangle BoundingRectangle { get; set; }

        public string FontName { get; set; }
        public bool IsSymbolic { get { return this.Flags.HasFlag(FontFlags.Symbolic); } }

        public bool IsBold
        {
            get
            {
                return this.FontName.IndexOf("bold", StringComparison.OrdinalIgnoreCase) >= 0 ||
                       this.Flags.HasFlag(FontFlags.Bold);
            }
        }

        public bool IsItalic
        {
            get
            {
                return this.FontName.IndexOf("italic", StringComparison.OrdinalIgnoreCase) >= 0 ||
                       this.FontName.IndexOf("oblique", StringComparison.OrdinalIgnoreCase) >= 0||
                       this.Flags.HasFlag(FontFlags.Italic);
            }
        }

        public int FontSize { get; set; }
        public string FontColor { get; set; }
        public int Rotation { get; set; }
        public int Angle { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public FontFlags Flags { get; set; }

        private void SetBoundingRectangle(Vector bottomLeft, Vector topRight)
        {
            var rectangle = new Rectangle(
                bottomLeft.X(),
                bottomLeft.Y(),
                topRight.X(),
                topRight.Y());

            this.BoundingRectangle = rectangle;
        }

        public static TokenBlock Create(string text, LineSegment lineSegment, Vector bottomLeft, Vector topRight)
        {
            var tokenBlock = new TokenBlock()
            {
                Text = text,
                Baseline = lineSegment,
            };

            tokenBlock.SetBoundingRectangle(bottomLeft, topRight);
            return tokenBlock;
        }

        public static TokenBlock CreateEmpty()
        {
            return new TokenBlock();
        }
    }
}