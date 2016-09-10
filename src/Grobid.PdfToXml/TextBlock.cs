﻿using System;
using System.Linq;

namespace Grobid.PdfToXml
{
    public class TextBlock
    {
        private readonly TokenBlock[] tokenBlocks;

        public TextBlock(TokenBlock[] tokenBlocks)
        {
            this.tokenBlocks = tokenBlocks;
        }

        public TokenBlock[] TokenBlocks => this.tokenBlocks;

        private TokenBlock First => this.tokenBlocks[0];
        private TokenBlock Last => this.tokenBlocks[this.tokenBlocks.Length - 1];

        public float X => this.First.X;
        public float Y => this.First.Y;

        public float Width => this.Last.BoundingRectangle.Right - this.First.BoundingRectangle.Left;
        public float Height => this.First.Height;
        public string Text => String.Join(" ", tokenBlocks.Select(x => x.Text));
    }
}
