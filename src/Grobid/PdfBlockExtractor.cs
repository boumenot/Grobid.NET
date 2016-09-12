using System;
using System.Collections.Generic;

using Grobid.PdfToXml;

namespace Grobid.NET
{
    public class PdfBlockExtractor<T>
    {
        private readonly BlockStateFactory factory;

        public PdfBlockExtractor()
        {
            this.factory = new BlockStateFactory();
        }

        public IEnumerable<T> Extract(IEnumerable<Block> blocks, Func<BlockState, T> transform)
        {
            foreach (var block in blocks)
            {
                foreach (var textBlock in block.TextBlocks)
                {
                    foreach (var tokenBlock in textBlock.TokenBlocks)
                    {
                        var blockState = this.factory.Create(block, textBlock, tokenBlock);
                        yield return transform(blockState);
                    }
                }
            }
        }
    }
}
