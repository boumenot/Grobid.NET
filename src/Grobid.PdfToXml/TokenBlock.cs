using System;
using System.Linq;

using iTextSharp.text;

namespace Grobid.PdfToXml
{
    public class TokenBlock
    {
        public static readonly TokenBlock Empty = new TokenBlock { IsEmpty = true };

        public int Id { get; set; }

        public float X { get; set; }
        public float Y { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
        public int Angle { get; set; }
        public float Base { get; set; }
        public Rectangle BoundingRectangle { get; set; }
        public string FontColor { get; set; }
        public FontFlags FontFlags { get; set; }
        public FontName FontName { get; set; }
        public float FontSize { get; set; }
        public bool IsEmpty { get; set; }
        public int Rotation { get; set; }
        public string Text { get; set; }

        public bool IsBold => this.FontName.IsBold || this.FontFlags.HasFlag(FontFlags.Bold);
        public bool IsItalic => this.FontName.IsItalic || this.FontFlags.HasFlag(FontFlags.Italic);
        public bool IsSymbolic => this.FontFlags.HasFlag(FontFlags.Symbolic);

        public static TokenBlock Merge(TokenBlock[] tokenBlocks)
        {
            var mergedTokenBlock = tokenBlocks[0];
            mergedTokenBlock.Text = String.Join(String.Empty, tokenBlocks.Select(x => x.Text)).Normalize();

            mergedTokenBlock.BoundingRectangle = new Rectangle(
                tokenBlocks.First().BoundingRectangle.Left,
                tokenBlocks.First().BoundingRectangle.Bottom,
                tokenBlocks.Last().BoundingRectangle.Right,
                tokenBlocks.Last().BoundingRectangle.Top);

            mergedTokenBlock.Width = mergedTokenBlock.BoundingRectangle.Width;

            return mergedTokenBlock;
        }

        public TokenBlock[] Tokenize()
        {
            var tokenBlocks = this
                .Text
                .SplitWithDelims(Constants.FullPunctuation)
                .Select(this.CloneWithText)
                .ToArray();

            return tokenBlocks;
        }

        private TokenBlock CloneWithText(string text)
        {
            return new TokenBlock
            {
                FontFlags = this.FontFlags,
                FontColor = this.FontColor,
                FontName = this.FontName,
                FontSize = this.FontSize,
                Angle = this.Angle,
                Rotation = this.Rotation,
                IsEmpty = this.IsEmpty,
                Base = this.Base,
                BoundingRectangle = this.BoundingRectangle,
                Height = this.Height,
                Width = this.Width,
                X = this.X,
                Y = this.Y,
                Text = text,
            };
        }
    }
}