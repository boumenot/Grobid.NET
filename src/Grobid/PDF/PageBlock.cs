using System.Collections.Generic;

namespace Grobid.NET
{
    public class PageBlock
    {
        public float Height { get; set; }
        public float Width { get; set; }
        public IReadOnlyList<TextInfo> TextInfos { get; set; }
    }
}