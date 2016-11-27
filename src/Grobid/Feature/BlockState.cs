using Grobid.PdfToXml;

namespace Grobid.NET.Feature
{
    public struct BlockState
    {
        public BlockStatus BlockStatus { get; set; }
        public LineStatus LineStatus { get; set; }
        public FontStatus FontStatus { get; set; }
        public FontSizeStatus FontSizeStatus { get; set; }

        public FontName FontName { get; set; }
        public bool IsBold => this.FontName.IsBold;
        public bool IsItalic => this.FontName.IsItalic;

        public string Text { get; set; }
    }
}