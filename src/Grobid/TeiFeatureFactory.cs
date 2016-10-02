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
            "/tei/text/front/date",
            "/tei/text/front/degree",
            "/tei/text/front/div[@type='abstract']",
            "/tei/text/front/div[@type='intro']",
            "/tei/text/front/div[@type='introduction']",
            //"/tei/text/front/div[@type='paragraph']",
            "/tei/text/front/docTitle/titlePart",
            "/tei/text/front/email",
            //"/tei/text/front/fileDesc[@xml:id]",
            "/tei/text/front/idno",
            //"/tei/text/front/keyword",
            "/tei/text/front/keywords",
            "/tei/text/front/note",
            //"/tei/text/front/note[@type='acknowledgement']",
            //"/tei/text/front/note[@type='copyright']",
            //"/tei/text/front/note[@type='copyrights']",
            //"/tei/text/front/note[@type='dedication']",
            //"/tei/text/front/note[@type='degree']",
            //"/tei/text/front/note[@type='english-title']",
            //"/tei/text/front/note[@type='grant']",
            //"/tei/text/front/note[@type='notification']",
            "/tei/text/front/note[@type='other']",
            "/tei/text/front/note[@type='phone']",
            "/tei/text/front/note[@type='reference']",
            "/tei/text/front/note[@type='submission']",
            "/tei/text/front/ptr[@type='web']",
            "/tei/text/front/reference",
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
                {"date", x => TeiFeatureFactory.Annotate("date", x) },
                {"date[@type='submission']", x => TeiFeatureFactory.Annotate("date-submission", x) },
                {"degree", x => TeiFeatureFactory.Annotate("degree", x) },
                {"docAuthor", x => TeiFeatureFactory.Annotate("author", x) },
                {"div[@type='abstract']", x => TeiFeatureFactory.Annotate("abstract", x) },
                {"div[@type='intro']", x => TeiFeatureFactory.Annotate("intro", x) },
                {"div[@type='introduction']", x => TeiFeatureFactory.Annotate("intro", x) },
                {"email", x => TeiFeatureFactory.Annotate("email", x) },
                {"keywords", x => TeiFeatureFactory.Annotate("keyword", x) },
                {"idno", x => TeiFeatureFactory.Annotate("pubnum", x) },
                {"note", TeiFeatureFactory.AnnotateNote },
                {"ptr[@type='web']", x => TeiFeatureFactory.Annotate("web", x) },
                {"reference", x => TeiFeatureFactory.Annotate("reference", x) },
                {"titlePart[@type='main']", x => TeiFeatureFactory.Annotate("title", x) },
                {"titlePart", x => TeiFeatureFactory.Annotate("title", x) },
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

        private static string[] AnnotateNote(XElement element)
        {
            string type = element.Attribute("type")?.Value ?? string.Empty;

            switch (type)
            {
                case "copyright":
                case "copyrights":
                    return TeiFeatureFactory.Annotate("copyright", element);
                case "degree":
                    return TeiFeatureFactory.Annotate("degree", element);
                case "dedication":
                    return TeiFeatureFactory.Annotate("dedication", element);
                case "english-title":
                    return TeiFeatureFactory.Annotate("entitle", element);
                case "grant":
                    return TeiFeatureFactory.Annotate("grant", element);
                case "other":
                    return TeiFeatureFactory.Annotate("note", element);
                case "phone":
                    return TeiFeatureFactory.Annotate("phone", element);
                case "reference":
                    return TeiFeatureFactory.Annotate("reference", element);
                case "submission":
                    return TeiFeatureFactory.Annotate("submission", element);
                default:
                    return TeiFeatureFactory.Annotate("note", element);
            }
        }

        /// <summary>
        /// Return a tuple of possible XPath Processor lookup keys.
        /// </summary>
        /// <para>
        /// The first tuple item is just the name of the element.  The second
        /// tuple item is the element name and attribute of type.
        /// </para>
        private static Tuple<string, string> ToFuncName(XElement element)
        {
            var attrValue = element.Attribute("type")?.Value;
            return Tuple.Create(
                $"{element.Name.LocalName}",
                $"{element.Name.LocalName}[@type='{attrValue}']");
        }

        public string Create(XDocument doc)
        {
            var strings = doc
                .XPathSelectElements(TeiFeatureFactory.XPathExpression)
                // Get the list of possible XPath Processor keys
                .Select(
                    x => new
                    {
                        Element = x,
                        FuncNames = TeiFeatureFactory.ToFuncName(x),
                    })
                // Get the processor used to compute the result.  Favor the more
                // *precise* key, which is of the form element[@type='value'].
                .Select(x => new
                {
                    x.Element,
                    Func = TeiFeatureFactory.XElementProcessor.ContainsKey(x.FuncNames.Item2) ?
                        TeiFeatureFactory.XElementProcessor[x.FuncNames.Item2] :
                        TeiFeatureFactory.XElementProcessor[x.FuncNames.Item1],
                })
                .SelectMany(x => x.Func(x.Element))
                .ToArray();

            return String.Join(Environment.NewLine, strings);
        }
    }
}