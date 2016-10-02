using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

using Grobid.PdfToXml;

namespace Grobid.NET
{
    public class TeiFeatureFactory
    {
        private static string[] XPathExpressions = new string[]
        {
            "/tei/text/front/address",
            //"/tei/text/front/biblScope[@type='pp']",
            //"/tei/text/front/biblScope[@type='vol']",
            "/tei/text/front/byline/affiliation",
            "/tei/text/front/byline/docAuthor",
            //"/tei/text/front/date",
            //"/tei/text/front/date[@type='submission']",
            //"/tei/text/front/degree",
            "/tei/text/front/div[@type='abstract']",
            "/tei/text/front/div[@type='intro']",
            "/tei/text/front/div[@type='introduction']",
            //"/tei/text/front/div[@type='paragraph']",
            "/tei/text/front/docTitle/titlePart",
            "/tei/text/front/email",
            //"/tei/text/front/fileDesc[@xml:id]",
            //"/tei/text/front/idno",
            //"/tei/text/front/keyword",
            "/tei/text/front/keywords",
            //"/tei/text/front/note",
            //"/tei/text/front/note[@type='acknowledgement']",
            "/tei/text/front/note[@type='copyright']",
            //"/tei/text/front/note[@type='dedication']",
            "/tei/text/front/note[@type='degree']",
            //"/tei/text/front/note[@type='english-title']",
            "/tei/text/front/note[@type='grant']",
            //"/tei/text/front/note[@type='notification']",
            "/tei/text/front/note[@type='other']",
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
                {"address", x => TeiFeatureFactory.Annotate("address", x) },
                {"affiliation", x => TeiFeatureFactory.Annotate("affiliation", x) },
                {"docAuthor", x => TeiFeatureFactory.Annotate("author", x) },
                {"div[@type='abstract']", x => TeiFeatureFactory.Annotate("abstract", x) },
                {"email", x => TeiFeatureFactory.Annotate("email", x) },
                {"div[@type='intro']", x => TeiFeatureFactory.Annotate("intro", x) },
                {"div[@type='introduction']", x => TeiFeatureFactory.Annotate("intro", x) },
                {"keywords", x => TeiFeatureFactory.Annotate("keyword", x) },
                {"note[@type='copyright']", x => TeiFeatureFactory.Annotate("copyright", x) },
                {"note[@type='degree']", x => TeiFeatureFactory.Annotate("degree", x) },
                {"note[@type='grant']", x => TeiFeatureFactory.Annotate("grant", x) },
                {"note[@type='other']", x => TeiFeatureFactory.Annotate("note", x) },
                {"titlePart", x => TeiFeatureFactory.Annotate("title", x) },
                {"titlePart[@type='main']", x => TeiFeatureFactory.Annotate("title", x) },
            };
        }

        private static string[] Annotate(string annotation, XElement element)
        {
            var xs = element
                .DescendantNodes()
                // HACK(1)
                .Select(x => x is XText ? ((XText)x).Value : ((XElement)x).Name == "lb" ? "@newline" : string.Empty)
                .SelectMany(x => x.SplitWithDelims(PdfToXml.Constants.FullPunctuation))
                // Usually we want to keep the delimiter, but in this case we ignore
                // the any whitespace because it is unnecessary.
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(
                    (x, i) => string.Format("{0} {1}{2}",
                        x,
                        i == 0 ? TeiFeatureFactory.OnePrefix : string.Empty,
                        x == "@newline" ? string.Empty : $"<{annotation}>"))
                // HACK(2)
                .Select(x => x.TrimEnd())
                .ToArray();

            return xs;
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