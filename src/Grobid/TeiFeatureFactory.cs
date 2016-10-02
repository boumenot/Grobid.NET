using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

using Grobid.PdfToXml;

namespace Grobid.NET
{
    public class TeiFeature
    {
        public string Value { get; set; }
        public string Classification { get; set; }
        public int TokenIndex { get; set; }
        public bool IsStart => this.TokenIndex == 0;

        internal bool IsControl => this.Value == "@newline";

        public static TeiFeature Create(string value, string classification, int tokenIndex)
        {
            var feature = new TeiFeature
            {
                Value = value,
                Classification = classification,
                TokenIndex = tokenIndex,
            };

            return feature;
        }
    }

    public class TeiFeatureFormatter
    {
        private static string OnePrefix = "I-";

        public string Format(TeiFeature x)
        {
            if (x.IsControl)
            {
                return x.Value;
            }

            return $"{x.Value} {(x.IsStart ? TeiFeatureFormatter.OnePrefix : string.Empty)}<{x.Classification}>";
        }

        public string[] Format(TeiFeature[] teiFeatures)
        {
            var strings = teiFeatures.Select(this.Format)
                .ToArray();

            return strings;
        }

        public string CreateString(TeiFeature[] teiFeatures)
        {
            return String.Join(Environment.NewLine, this.Format(teiFeatures));
        }
    }

    public class TeiFeatureFactory
    {
        private static string[] XPathExpressions = new string[]
        {
            "/tei/text/front/address",
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
            //"/tei/text/front/note[@type='other']",
            //"/tei/text/front/note[@type='phone']",
            //"/tei/text/front/note[@type='reference']",
            //"/tei/text/front/note[@type='submission']",

            "/tei/text/front/ptr[@type='web']",
            "/tei/text/front/reference",

            // TBD: not needed so therefore not implemented yet
            //"/citations/bibl/biblScope[@type='pp']",
            //"/citations/bibl/biblScope[@type='vol']",
            //"/citations/bibl/title",
            //"/tei/text/front/web",
        };

        private static Dictionary<string, Func<XElement, TeiFeature[]>> XElementProcessor;

        private static string XPathExpression;

        static TeiFeatureFactory()
        {
            TeiFeatureFactory.XPathExpression = string.Join("|", TeiFeatureFactory.XPathExpressions);
            TeiFeatureFactory.XElementProcessor = new Dictionary<string, Func<XElement, TeiFeature[]>>
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

        private static TeiFeature[] Annotate(string annotation, XElement element)
        {
            var xs = element
                .DescendantNodes()
                // HACK(1)
                .Select(x => x is XText ? ((XText)x).Value : ((XElement)x).Name == "lb" ? "@newline" : string.Empty)
                // Usually we want to keep the delimiter, but in this case we ignore
                // any whitespace because it is unnecessary.
                .SelectMany(x => x.SplitWithDelims(PdfToXml.Constants.FullPunctuation))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select((x, i) => TeiFeature.Create(x, annotation, i))
                .ToArray();

            return xs;
        }

        private static TeiFeature[] AnnotateNote(XElement element)
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
                //case "other":
                //    return TeiFeatureFactory.Annotate("note", element);
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

        public TeiFeature[] Create(XDocument doc)
        {
            var features = doc
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

            return features;
        }
    }
}