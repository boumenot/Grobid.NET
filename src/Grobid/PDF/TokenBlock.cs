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
            var textInfo = new TokenBlock()
            {
                Text = text,
                Baseline = lineSegment,
            };

            textInfo.SetBoundingRectangle(bottomLeft, topRight);
            return textInfo;
        }

        public static TokenBlock CreateEmpty()
        {
            return new TokenBlock();
        }
    }
}