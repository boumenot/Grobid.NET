namespace Grobid.PdfToXml
{
    public class PageBlock
    {
        public float Height { get; set; }
        public float Width { get; set; }
        public Block[] Blocks { get; set; }
        //public TextBlock[] TextBlocks { get; set; }
        public int Offset { get; set; }
    }
}