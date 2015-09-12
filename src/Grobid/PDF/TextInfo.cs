using System;

using iTextSharp.text;
using iTextSharp.text.pdf.parser;

namespace Grobid.NET
{
    public class TextInfo
    {
        private LineSegment Baseline { get; set; }

        public string Text { get; set; }

        public Vector StartPoint
        {
            get { return this.Baseline.GetStartPoint(); }
        }

        public Vector EndPoint
        {
            get { return this.Baseline.GetStartPoint(); }
        }

        public Rectangle BoundingRectangle { get; set; }

        private void SetBoundingRectangle(Vector bottomLeft, Vector topRight)
        {
            var rectangle = new Rectangle(
                bottomLeft.X(),
                bottomLeft.Y(),
                topRight.X(),
                topRight.Y());

            this.BoundingRectangle = rectangle;
        }

        public static TextInfo Create(string text, LineSegment lineSegment, Vector bottomLeft, Vector topRight)
        {
            var textInfo = new TextInfo()
            {
                Text = text,
                Baseline = lineSegment,
            };

            textInfo.SetBoundingRectangle(bottomLeft, topRight);
            return textInfo;
        }

        public static TextInfo CreateEmpty()
        {
            return new TextInfo();
        }
    }
}