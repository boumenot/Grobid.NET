using System;
using System.Collections.Generic;
using System.Linq;

using Grobid.PdfToXml;

namespace Grobid.NET
{
    public class PdfBlockExtractor<T>
    {
        public IEnumerable<T> Extract(IEnumerable<Block> blocks, Func<BlockState, T> transform)
        {
            return Enumerable.Empty<T>();
        }
    }
}
