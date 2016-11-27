using System.Collections.Generic;
using System.Linq;

using Grobid.PdfToXml;

namespace Grobid.NET
{
    /// <summary>
    /// GROBID further tokenizes TokenBlocks, which causes the blocks extracted
    /// from a PDF document to be "reflowed."  This class tokenizes blocks based
    /// on GROBID's expectations, and re-flows (re-writes) the blocks.
    /// 
    /// The code could be smarter, and avoid recalculating blocks where it is not
    /// necessary.
    /// </summary>
    public sealed class BlockReflower
    {
        public IEnumerable<Block> Reflow(IEnumerable<Block> blocks)
        {
            foreach (var block in blocks)
            {
                var textBlocks = new List<TextBlock>();

                foreach (var textBlock in block.TextBlocks)
                {
                    var tokenBlocks = textBlock.TokenBlocks.SelectMany(x => x.Tokenize())
                        .ToArray();

                    textBlocks.Add(
                        new TextBlock(tokenBlocks, textBlock.Id));
                }

                yield return new Block { Id = block.Id, TextBlocks = textBlocks.ToArray() };
            }
        }
    }
}