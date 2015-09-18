using System.Threading;

using FluentAssertions;

using Xunit;

namespace Grobid.PdfToXml.Test
{
    public class TokenFactoryTest
    {
        [Fact]
        public void Test()
        {
            var testSubject = new TokenBlockFactory(0f, 0f);
            var tokenBlock = new TokenBlock();
        }
    }
}
