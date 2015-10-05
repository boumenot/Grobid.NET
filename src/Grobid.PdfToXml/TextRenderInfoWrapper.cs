using System.Collections.Generic;

using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace Grobid.PdfToXml
{
    internal class TextRenderInfoWrapper : ITextRenderInfo
    {
        private readonly TextRenderInfo textRenderInfo;

        public TextRenderInfoWrapper(TextRenderInfo textRenderInfo)
        {
            this.textRenderInfo = textRenderInfo;
        }

        public PdfString PdfString
        {
            get { return this.textRenderInfo.PdfString; }
        }

        public LineSegment GetAscentLine()
        {
            return this.textRenderInfo.GetAscentLine();
        }

        public LineSegment GetBaseline()
        {
            return this.textRenderInfo.GetBaseline();
        }

        public IList<TextRenderInfo> GetCharacterRenderInfos()
        {
            return this.textRenderInfo.GetCharacterRenderInfos();
        }

        public LineSegment GetDescentLine()
        {
            return this.textRenderInfo.GetDescentLine();
        }

        public BaseColor GetFillColor()
        {
            return this.textRenderInfo.GetFillColor();
        }

        public DocumentFont GetFont()
        {
            return this.textRenderInfo.GetFont();
        }

        public int? GetMcid()
        {
            return this.textRenderInfo.GetMcid();
        }

        public float GetRise()
        {
            return this.textRenderInfo.GetRise();
        }

        public float GetSingleSpaceWidth()
        {
            return this.textRenderInfo.GetSingleSpaceWidth();
        }

        public BaseColor GetStrokeColor()
        {
            return this.textRenderInfo.GetStrokeColor();
        }

        public string GetText()
        {
            return this.textRenderInfo.GetText();
        }

        public int GetTextRenderMode()
        {
            return this.textRenderInfo.GetTextRenderMode();
        }

        public LineSegment GetUnscaledBaseline()
        {
            return this.textRenderInfo.GetUnscaledBaseline();
        }

        public bool HasMcid(int mcid)
        {
            return this.textRenderInfo.HasMcid(mcid);
        }

        public bool HasMcid(int mcid, bool checkTheTopmostLevelOnly)
        {
            return this.textRenderInfo.HasMcid(mcid, checkTheTopmostLevelOnly);
        }
    }
}
