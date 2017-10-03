using System;
using System.Linq;
using Grobid.NET.Feature;
using Grobid.PdfToXml;

namespace Grobid.NET
{
    public class RawBlockStateFactory
    {
        private readonly Func<string, BlockState> factory;

        public RawBlockStateFactory(Func<string, BlockState> factory)
        {
            this.factory = factory;
        }

        public RawBlockStateFactory()
            : this(x => new BlockState
            {
                BlockStatus = BlockStatus.BLOCKIN,
                FontStatus = FontStatus.SAMEFONT,
                LineStatus = LineStatus.LINEIN,
                FontSizeStatus = FontSizeStatus.SAMEFONTSIZE,
                Text = x,
            })
        {
        }

        public BlockState[] Create(string text)
        {
            var blocks = text
                .SplitWithDelims(PdfToXml.Constants.FullPunctuation)
                .Select(this.factory)
                .ToArray();

            return blocks;
        }

        public BlockState[] CreateWithoutPunctuation(string text)
        {
            var blocks = text
                .Split(PdfToXml.Constants.FullPunctuation)
                .Select(this.factory)
                .ToArray();

            return blocks;
        }

    }
}
