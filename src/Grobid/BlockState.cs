namespace Grobid.NET
{
    public struct BlockState
    {
        public BlockStatus BlockStatus { get; set; }
        public LineStatus LineStatus { get; set; }
        public FontStatus FontStatus { get; set; }
        public FontSizeStatus FontSizeStatus { get; set; }
        public string Text { get; set; }
    }
}