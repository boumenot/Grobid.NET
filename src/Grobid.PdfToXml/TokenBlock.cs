using System;
using System.Linq;

using iTextSharp.text;

namespace Grobid.PdfToXml
{
    public class TokenBlock
    {
        public static readonly TokenBlock Empty = new TokenBlock { IsEmpty = true };
        
        public int Angle { get; set; }
        public float Base { get; set; }
        public Rectangle BoundingRectangle { get; set; }
        public string FontColor { get; set; }
        public FontFlags FontFlags { get; set; }
        public FontName FontName { get; set; }
        public float FontSize { get; set; }
        public float Height { get; set; }
        public bool IsEmpty { get; set; }
        public int Rotation { get; set; }
        public string Text { get; set; }
        public float Width { get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        public bool IsBold => this.FontName.IsBold || this.FontFlags.HasFlag(FontFlags.Bold);
        public bool IsItalic => this.FontName.IsItalic || this.FontFlags.HasFlag(FontFlags.Italic);
        public bool IsSymbolic => this.FontFlags.HasFlag(FontFlags.Symbolic);

        public static TokenBlock Merge(TokenBlock[] tokenBlocks)
        {
            var mergedTokenBlock = tokenBlocks[0];
            mergedTokenBlock.Text = String.Join(String.Empty, tokenBlocks.Select(x => x.Text));

            return mergedTokenBlock;
        }
    }
}