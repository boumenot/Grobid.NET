using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

using ApprovalTests;
using ApprovalTests.Core;
using Xunit;

namespace Grobid.Test
{
    public class TeiFeatureFactoryTest
    {
        [Fact]
        public void TeiFeatureFactoryTitle00()
        {
            var tei = this.InsertXmlSnippetIntoTei("<docTitle><titlePart>The wizard quickly jinxed the gnomes before they vaporized</titlePart></docTitle>");

            var testSubject = new TeiFeatureFactory();
            Approvals.Verify(testSubject.Create(XDocument.Parse(tei)));
        }

        [Fact]
        public void TeiFeatureFactoryTitle01()
        {
            var tei = this.InsertXmlSnippetIntoTei("<docTitle><titlePart type='main'>The wizard quickly jinxed the gnomes before they vaporized</titlePart></docTitle>");

            var testSubject = new TeiFeatureFactory();
            Approvals.Verify(testSubject.Create(XDocument.Parse(tei)));
        }


        private string InsertXmlSnippetIntoTei(string s)
        {
            var tei = $@"
<?xml version='1.0' encoding='utf-8'?>
<tei>
  <text xml:lang='en'>
    <front>
     {s}
    </front>
  </text>
</tei>";
            return tei.Trim();
        }
    }

    public class TeiFeatureFactory
    {
        private static string[] XPathExpressions = new string[]
        {
            //"/tei/text/front/address",
            //"/tei/text/front/affiliation",
            //"/tei/text/front/biblScope[@type='pp']",
            //"/tei/text/front/biblScope[@type='vol']",
            //"/tei/text/front/byline/affiliation",
            //"/tei/text/front/byline/docAuthor",
            //"/tei/text/front/date",
            //"/tei/text/front/date[@type='submission']",
            //"/tei/text/front/degree",
            //"/tei/text/front/div[@type='abstract']",
            //"/tei/text/front/div[@type='intro']",
            //"/tei/text/front/div[@type='introduction']",
            //"/tei/text/front/div[@type='paragraph']",
            "/tei/text/front/docTitle/titlePart",
            //"/tei/text/front/email",
            //"/tei/text/front/fileDesc[@xml:id]",
            //"/tei/text/front/idno",
            //"/tei/text/front/keyword",
            //"/tei/text/front/keywords",
            //"/tei/text/front/note",
            //"/tei/text/front/note[@type='acknowledgement']",
            //"/tei/text/front/note[@type='copyright']",
            //"/tei/text/front/note[@type='dedication']",
            //"/tei/text/front/note[@type='degree']",
            //"/tei/text/front/note[@type='english-title']",
            //"/tei/text/front/note[@type='grant']",
            //"/tei/text/front/note[@type='notification']",
            //"/tei/text/front/note[@type='other']",
            //"/tei/text/front/note[@type='phone']",
            //"/tei/text/front/note[@type='reference']",
            //"/tei/text/front/note[@type='submission']",
            //"/tei/text/front/ptr[@type='web']",
            //"/tei/text/front/reference",
            //"/tei/text/front/title",
            //"/tei/text/front/web",
        };

        private static string OnePrefix = "I-";
        private static Dictionary<string, Func<XElement, string[]>> XElementProcessor;

        private static string XPathExpression;

        static TeiFeatureFactory()
        {
            TeiFeatureFactory.XPathExpression = string.Join("|", TeiFeatureFactory.XPathExpressions);
            TeiFeatureFactory.XElementProcessor = new Dictionary<string, Func<XElement, string[]>>
            {
                {"titlePart", TeiFeatureFactory.ExtractTitle},
                {"titlePart[@type='main']", TeiFeatureFactory.ExtractTitle},
            };
        }

        private static string[] Annotate(string annotation, string[] elements)
        {
            return elements
                .Select(
                    (x, i) => $"{x} {(i == 0 ? TeiFeatureFactory.OnePrefix : string.Empty)}<{annotation}>")
                .ToArray();
        }

        private static string[] ExtractTitle(XElement arg)
        {
            return TeiFeatureFactory.Annotate("title", arg.Value.Split(' '));
        }

        private static string ToFuncName(XElement element)
        {
            var attr = element.Attribute("type");
            var value = attr == null
                       ? $"{element.Name.LocalName}"
                       : $"{element.Name.LocalName}[@type='{attr.Value}']";

            return value;
        }

        public string Create(XDocument doc)
        {
            var strings = doc
                .XPathSelectElements(TeiFeatureFactory.XPathExpression)
                .Select(
                    x => new
                    {
                        Element = x,
                        Func = TeiFeatureFactory.XElementProcessor[TeiFeatureFactory.ToFuncName(x)],
                    })
                .SelectMany(x => x.Func(x.Element))
                .ToArray();

            return String.Join(Environment.NewLine, strings);
        }
    }
}
