using System;

using FluentAssertions;

using Xunit;

namespace Grobid.PdfToXml.Test
{
    public class TokenBlockFactoryTest
    {
        [Fact]
        public void TestA()
        {
            var stub = new TextRenderInfoStub();
            var testSubject = new TokenBlockFactory(100, 100);

            Action test = () => testSubject.Create(stub);
            test.ShouldThrow<NullReferenceException>("I am still implementing the code.");
        }
    }
}
