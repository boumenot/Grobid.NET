using System.Collections.Generic;

using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace Grobid.PdfToXml.Test
{
    public class TextRenderInfoStub : ITextRenderInfo
    {
        public PdfString PdfString { get; set; }

        public LineSegment GetAscentLine()
        {
            return this.AscentLine;
        }

        public LineSegment GetBaseline()
        {
            return this.Baseline;
        }

        public IList<TextRenderInfo> GetCharacterRenderInfos()
        {
            return this.CharaRenderInfos;
        }

        public LineSegment GetDescentLine()
        {
            return this.DescentLine;
        }

        public BaseColor GetFillColor()
        {
            return this.FillColor;
        }

        public DocumentFont GetFont()
        {
            return this.Font;
        }

        public int? GetMcid()
        {
            return this.Mcid;
        }

        public float GetRise()
        {
            return this.Rise;
        }

        public float GetSingleSpaceWidth()
        {
            return this.SingleSpaceWidth;
        }

        public BaseColor GetStrokeColor()
        {
            return this.StrokeColor;
        }

        public string GetText()
        {
            return this.Text;
        }

        public int GetTextRenderMode()
        {
            return this.TextRenderMode;
        }

        public LineSegment GetUnscaledBaseline()
        {
            return this.UnscaledBaseline;
        }

        public bool HasMcid(int mcid)
        {
            return this.HasMcidStub;
        }

        public bool HasMcid(int mcid, bool checkTheTopmostLevelOnly)
        {
            return this.HasMcidStub;
        }

        #region Stubs

        public LineSegment AscentLine { get; set; }
        public LineSegment Baseline { get; set; }
        public IList<TextRenderInfo> CharaRenderInfos { get; set; }
        public LineSegment DescentLine { get; set; }
        public BaseColor FillColor { get; set; }
        public DocumentFont Font { get; set; }
        public int? Mcid { get; set; }
        public float Rise { get; set; }
        public float SingleSpaceWidth { get; set; }
        public BaseColor StrokeColor { get; set; }
        public string Text { get; set; }
        public int TextRenderMode { get; set; }
        public LineSegment UnscaledBaseline { get; set; }
        public bool HasMcidStub { get; set; }

        #endregion
    }
}