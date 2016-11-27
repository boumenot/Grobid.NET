using Grobid.PdfToXml;

namespace Grobid.NET
{
    public sealed class IntroductionFilter
    {
        public bool IsIntroduction(Block arg)
        {
            return DocumentStructure.IntroductionStrict.IsMatch(arg.Text);
        }
    }
}