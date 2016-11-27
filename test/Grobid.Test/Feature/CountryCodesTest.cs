using FluentAssertions;

using Grobid.NET.Feature;

using Xunit;

namespace Grobid.Test.Feature
{
    public class CountryCodesTest
    {
        [Fact]
        public void Test()
        {
            var doc = @"<?xml version='1.0' encoding='UTF-8'?>
<?xml-stylesheet type='text/xsl' href='completeCodes.xsl'?>
<TEI xmlns='http://www.tei-c.org/ns/1.0' xmlns:xlink='http://www.w3.org/1999/xlink'
    xmlns:mml='http://www.w3.org/1998/Math/MathML' xmlns:tei='http://www.tei-c.org/ns/1.0'>
    <teiHeader>
        <fileDesc>
            <titleStmt>
                <title>Reference table for country codes (ISO 3166 a2 and a3)</title>
            </titleStmt>
            <publicationStmt>
                <p>...</p>
            </publicationStmt>
            <sourceDesc>
                <p>...</p>
            </sourceDesc>
        </fileDesc>
    </teiHeader>
    <text>
        <body>
            <div>
                <table>
                    <row>
                        <cell role='a2code'>AF</cell>
                        <cell role='a3code'>AFG</cell>
                        <cell role='name' xml:lang='fr'>AFGHANISTAN</cell>
                        <cell role='name' xml:lang='en'>AFGHANISTAN</cell>
                        <cell role='nameAlt' xml:lang='en'>Afghanistan</cell>
                    </row>
                </table>
            </div>
        </body>
    </text>
</TEI>";
            var testSubject = CountryCodes.FromTei(doc.ToStream());
            testSubject.GetCode("afghanistan").Should().Be("AF");
            testSubject.GetCode("AFGHANISTAN").Should().Be("AF");
            testSubject.GetCode("--does-not-exist").Should().BeNull();
        }
    }
}
