using System;
using System.Collections.Generic;
using System.Linq;

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
            return from block in blocks
                   from textBlock in block.TextBlocks
                   let tokenBlocks = textBlock.TokenBlocks.SelectMany(x => x.Tokenize())
                   from tokenBlock in tokenBlocks
                   select this.factory.Create(block, textBlock, tokenBlock)
                   into blockState
                   select transform(blockState);
        }
    }
}
