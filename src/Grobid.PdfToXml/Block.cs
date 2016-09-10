using System;
using System.Linq;

namespace Grobid.PdfToXml
{
    public class Block
    {
        public FontName FontName => this.First.FontName;
        public float FontSize => this.First.FontSize;
        public string FontColor => this.First.FontColor;

        public bool IsBold => this.First.IsBold;
        public bool IsItalic => this.First.IsItalic;

        public float Height => this.First.Height;
        public float Width => this.First.Width;
        public float X => this.First.X;
        public float Y => this.First.Y;

        public string Text => String.Join("\n", this.TextBlocks.Select(x => x.Text));

        public int Page { get; set; }
        public TextBlock[] TextBlocks { get; set; }

        private TokenBlock First => this.TextBlocks[0].TokenBlocks[0];
    }
}
