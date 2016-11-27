using System;

namespace Grobid.PdfToXml
{
    public sealed class IndexGenerator
    {
        private int index;
        private int pageIndex;
        private int blockIndex;
        private int textIndex;
        private int tokenIndex;

        public string PageIndex
        {
            get
            {
                this.index++;
                this.pageIndex++;
                this.textIndex = 0;
                this.tokenIndex = 0;
                this.blockIndex = 0;
                return $"p{this.pageIndex}";
            }
        }

        public string TextIndex
        {
            get
            {
                this.index++;
                this.textIndex++;
                return $"p{this.pageIndex}_t{this.textIndex}";
            }
        }

        public string TokenIndex
        {
            get
            {
                this.tokenIndex++;
                return $"p{this.pageIndex}_w{this.tokenIndex}";
            }
        }

        public string SidIndex
        {
            get
            {
                this.index++;
                return $"p{this.pageIndex}_s{this.index}";
            }
        }

        public string BlockIndex
        {
            get
            {
                this.blockIndex++;
                return $"p{this.pageIndex}_b{this.blockIndex}";
            }
        }
    }
}