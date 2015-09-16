using System;

namespace Grobid.PdfToXml
{
    public class IndexGenerator
    {
        private int index;
        private int pageIndex;
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
                return String.Format("p{0}", this.pageIndex);
            }
        }

        public string TextIndex
        {
            get
            {
                this.index++;
                this.textIndex++;
                return String.Format("p{0}_t{1}", this.pageIndex, this.textIndex);
            }
        }

        public string TokenIndex
        {
            get
            {
                this.tokenIndex++;
                return String.Format("p{0}_w{1}", this.pageIndex, this.tokenIndex);
            }
        }

        public string SidIndex
        {
            get
            {
                this.index++;
                return String.Format("p{0}_s{1}", this.pageIndex, this.index);
            }
        }
    }
}