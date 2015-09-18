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

        public TokenBlock[] TokenBlocks { get { return this.tokenBlocks; } }

        private TokenBlock First
        {
            get { return this.TokenBlocks[0]; }
        }

        private TokenBlock Last
        {
            get { return this.tokenBlocks[this.tokenBlocks.Length - 1]; }
        }

        public float X
        {
            get { return this.First.X; }
        }

        public float Y
        {
            get { return this.First.Y; }
        }

        public float Width
        {
            get { return this.Last.BoundingRectangle.Right- this.First.BoundingRectangle.Left; }
        }

        public float Height
        {
            get { return this.First.Height; }
        }

        public string Text
        {
            get { return String.Join(" ", tokenBlocks.Select(x => x.Text)); }
        }
    }
}
