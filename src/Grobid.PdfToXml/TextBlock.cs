﻿using System;
using System.Linq;

namespace Grobid.PdfToXml
{
    public class TextBlock
    {
        private readonly float pageHeight;
        private readonly TokenBlock[] tokenBlocks;

        public TextBlock(TokenBlock[] tokenBlocks, float pageHeight)
        {
            this.tokenBlocks = tokenBlocks;
            this.pageHeight = pageHeight;
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
            get { return this.First.StartPoint.X(); }
        }

        public float Y
        {
            get { return this.pageHeight - this.First.BoundingRectangle.Top; }
        }

        public float Width
        {
            get { return this.Last.EndPoint.X() - this.First.StartPoint.X(); }
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
