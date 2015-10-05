using System.Collections.Generic;

using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace Grobid.PdfToXml
{
    public interface ITextRenderInfo
    {
        #region Code for iTextSharp.text.pdf.parser.TextRenderInfo 

        PdfString PdfString { get; }

        LineSegment GetAscentLine();

        LineSegment GetBaseline();

        IList<TextRenderInfo> GetCharacterRenderInfos();

        LineSegment GetDescentLine();

        BaseColor GetFillColor();

        // XXX(boumenot): this method makes it hard to stub, so I am removing it
        // the necessary methods have been exposed as distinct methods.  See
        // "font" region.
        // DocumentFont GetFont();

        int? GetMcid();

        float GetRise();

        float GetSingleSpaceWidth();

        BaseColor GetStrokeColor();

        string GetText();

        int GetTextRenderMode();

        LineSegment GetUnscaledBaseline();

        bool HasMcid(int mcid);

        bool HasMcid(int mcid, bool checkTheTopmostLevelOnly);

        #endregion

        #region Explicit Font Methods

        float GetFontAscent();
        float GetFontDescent();
        int GetFontFlags();
        string GetPostscriptFontName();

        #endregion
    }
}
