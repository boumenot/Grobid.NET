using System.Collections.Generic;

using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace Grobid.PdfToXml
{
    public interface ITextRenderInfo
    {
        PdfString PdfString { get; }

        LineSegment GetAscentLine();

        LineSegment GetBaseline();

        IList<TextRenderInfo> GetCharacterRenderInfos();

        LineSegment GetDescentLine();

        BaseColor GetFillColor();

        DocumentFont GetFont();

        int? GetMcid();

        float GetRise();

        float GetSingleSpaceWidth();

        BaseColor GetStrokeColor();

        string GetText();

        int GetTextRenderMode();

        LineSegment GetUnscaledBaseline();

        bool HasMcid(int mcid);

        bool HasMcid(int mcid, bool checkTheTopmostLevelOnly);
    }
}
