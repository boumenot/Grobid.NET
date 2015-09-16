using System.Collections.Generic;

namespace Grobid.PdfToXml
{
    public class PageBlock
    {
        public float Height { get; set; }
        public float Width { get; set; }
        public IReadOnlyList<TokenBlock> TokenBlocks { get; set; }
    }
}