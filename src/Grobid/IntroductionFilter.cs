using Grobid.PdfToXml;

namespace Grobid.NET
{
    public class IntroductionFilter
    {
        public bool IsIntroduction(Block arg)
        {
            return DocumentStructure.IntroductionStrict.IsMatch(arg.Text);
        }
    }
}