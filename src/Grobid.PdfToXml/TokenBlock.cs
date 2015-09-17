using System;
using System.Linq;

using iTextSharp.text;
using iTextSharp.text.pdf.parser;

namespace Grobid.PdfToXml
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

        public FontName FontName { get; set; }
        public bool IsSymbolic { get { return this.Flags.HasFlag(FontFlags.Symbolic); } }

        public bool IsBold
        {
            get { return this.FontName.IsBold || this.Flags.HasFlag(FontFlags.Bold); }
        }

        public bool IsItalic
        {
            get { return this.FontName.IsItalic || this.Flags.HasFlag(FontFlags.Italic); }
        }

        public float FontSize { get { return this.BoundingRectangle.Height; } }
        public string FontColor { get; set; }
        public int Rotation { get { return 0; } }
        public int Angle { get { return 0; } }

        public float X
        {
            get { return this.StartPoint.X(); }
        }

        public float Width
        {
            get { return this.EndPoint.X() - this.StartPoint.X(); }
        }

        public float Y { get; set; }
        public float Height { get; set; }

        public FontFlags Flags { get; set; }
        public float Base { get; set; }

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

        public static TokenBlock Merge(TokenBlock[] tokenBlocks)
        {
            var mergedTokenBlock = tokenBlocks[0];
            mergedTokenBlock.Text = String.Join(String.Empty, tokenBlocks.Select(x => x.Text));

            return mergedTokenBlock;
        }
    }
}