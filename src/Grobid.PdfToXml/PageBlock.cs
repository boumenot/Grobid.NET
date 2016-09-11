namespace Grobid.PdfToXml
{
    public class PageBlock
    {
        public int Id { get; set; }

        public int Offset { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
        public Block[] Blocks { get; set; }
    }
}