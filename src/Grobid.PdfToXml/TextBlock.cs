﻿using System;
using System.Linq;

namespace Grobid.PdfToXml
{
    public sealed class TextBlock
    {
        public TextBlock(TokenBlock[] tokenBlocks, int id = 0)
        {
            this.TokenBlocks = tokenBlocks;
            this.Id = id;
        }

        public int Id { get; }

        public float X => this.First.X;
        public float Y => this.First.Y;

        public float Height => this.First.Height;
        public float Width => this.Last.BoundingRectangle.Right - this.First.BoundingRectangle.Left;
        public string Text => String.Join(" ", this.TokenBlocks.Select(x => x.Text));

        public TokenBlock[] TokenBlocks { get; }

        private TokenBlock First => this.TokenBlocks[0];
        private TokenBlock Last => this.TokenBlocks[this.TokenBlocks.Length - 1];
    }
}
