using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grobid.PdfToXml;

using Xunit;

namespace Grobid.Test
{
    public class IntroductionFilterTest
    {
        [Fact]
        public void Test()
        {
            var tokenBlock = new TokenBlock {  Text = "Something" };
            var textBlock = new TextBlock(new[] {tokenBlock}, 0);
            var block = new Block { TextBlocks = new [] { textBlock }} ;

            var testSubject = new IntroudctionFilter();
            var blocks = new[] { block };

        }
    }

    public class IntroudctionFilter {}
}
