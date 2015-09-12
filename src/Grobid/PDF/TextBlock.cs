using System;
using System.Text;

namespace Grobid.NET
{
    public class TextBlock
    {
        private readonly float pageHeight;
        private readonly TextInfo[] textInfos;

        public TextBlock(TextInfo[] textInfos, float pageHeight)
        {
            this.textInfos = textInfos;
            this.pageHeight = pageHeight;
        }

        private TextInfo First
        {
            get { return this.textInfos[0]; }
        }

        private TextInfo Last
        {
            get { return this.textInfos[this.textInfos.Length - 1]; }
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
            foreach (var textInfo in this.textInfos)
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
