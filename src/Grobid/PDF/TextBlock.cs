using System;
using System.Text;

namespace Grobid.NET
{
    public class TextBlock
    {
        private readonly float pageHeight;
        private readonly TokenBlock[] tokenBlocks;

        public TextBlock(TokenBlock[] tokenBlocks, float pageHeight)
        {
            this.tokenBlocks = tokenBlocks;
            this.pageHeight = pageHeight;
        }

        private TokenBlock First
        {
            get { return this.tokenBlocks[0]; }
        }

        private TokenBlock Last
        {
            get { return this.tokenBlocks[this.tokenBlocks.Length - 1]; }
        }

        public int X
        {
            get { return (int)Math.Round(this.First.StartPoint.X()); }
        }

        public int Y
        {
            get { return (int)Math.Round(this.pageHeight - this.First.BoundingRectangle.Top); }
        }

        public int Width
        {
            get { return (int)Math.Round(this.Last.EndPoint.X() - this.First.StartPoint.X()); }
        }

        public int Height
        {
            get { return (int)Math.Round(this.First.BoundingRectangle.Height); }
        }

        public string Text
        {
            get { return this.GetText(); }
        }

        public string GetText()
        {
            var sb = new StringBuilder();
            foreach (var textInfo in this.tokenBlocks)
            {
                var s = textInfo.IsEmpty
                            ? " "
                            : textInfo.Text;

                sb.Append(s);
            }

            return sb.ToString();
        }
    }
}
