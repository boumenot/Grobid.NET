using System;

using Grobid.PdfToXml;

namespace Grobid.NET
{
    public class BlockStateFactory
    {
        private string previousFullFontName;
        private float previousFontSize;

        public BlockStateFactory()
        {
            this.previousFullFontName = null;
            this.previousFontSize = -1f;
        }

        public BlockState Create(Block block, TextBlock textBlock, TokenBlock tokenBlock)
        {
            var blockState = new BlockState
            {
                BlockStatus = this.GetBlockStatus(block, tokenBlock),
                LineStatus = this.GetLineStatus(textBlock, tokenBlock),
                FontSizeStatus = this.GetFontSizeStatus(tokenBlock.FontSize),
                FontStatus = this.GetFontStatus(tokenBlock.FontName.FullName),
                Text = tokenBlock.Text,
            };

            this.previousFullFontName = tokenBlock.FontName.FullName;
            this.previousFontSize = tokenBlock.FontSize;

            return blockState;
        }

        private FontStatus GetFontStatus(string fullFontName)
        {
            if (StringComparer.OrdinalIgnoreCase.Compare(this.previousFullFontName, fullFontName) == 0)
            {
                return FontStatus.SAMEFONT;
            }

            this.previousFullFontName = fullFontName;
            return FontStatus.NEWFONT;
        }

        private FontSizeStatus GetFontSizeStatus(float fontSize)
        {
            // TODO: comparison of floats requires an epsilon consideration

            if (this.previousFontSize < fontSize)
            {
                return FontSizeStatus.HIGHERFONT;
            }

            if (this.previousFontSize > fontSize)
            {
                return FontSizeStatus.LOWFONT;
            }

            return FontSizeStatus.SAMEFONTSIZE;
        }

        private BlockStatus GetBlockStatus(Block block, TokenBlock tokenBlock)
        {
            return this.IsTokenBeginOfBlock(block, tokenBlock)
                       ? BlockStatus.BLOCKSTART
                       : this.IsTokenEndOfBlock(block, tokenBlock)
                           ? BlockStatus.BLOCKEND
                           : BlockStatus.BLOCKIN;
        }

        private bool IsTokenBeginOfBlock(Block block, TokenBlock tokenBlock)
        {
            var firstTextBlock = block.TextBlocks[0];
            return this.IsTokenBeginOfLine(firstTextBlock, tokenBlock);
        }

        private bool IsTokenEndOfBlock(Block block, TokenBlock tokenBlock)
        {
            var lastTextBlock = block.TextBlocks[block.TextBlocks.Length - 1];
            return this.IsTokenEndOfLine(lastTextBlock, tokenBlock);
        }

        private LineStatus GetLineStatus(TextBlock textBlock, TokenBlock tokenBlock)
        {
            return this.IsTokenBeginOfLine(textBlock, tokenBlock)
                       ? LineStatus.LINESTART
                       : this.IsTokenEndOfLine(textBlock, tokenBlock)
                           ? LineStatus.LINEEND
                           : LineStatus.LINEIN;
        }


        private bool IsTokenBeginOfLine(TextBlock textBlock, TokenBlock tokenBlock)
        {
            var firstToken = textBlock.TokenBlocks[0];
            return firstToken == tokenBlock;
        }

        private bool IsTokenEndOfLine(TextBlock textBlock, TokenBlock tokenBlock)
        {
            var lastToken = textBlock.TokenBlocks[textBlock.TokenBlocks.Length - 1];
            return lastToken == tokenBlock;
        }
    }
}