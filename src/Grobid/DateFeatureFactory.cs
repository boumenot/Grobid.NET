using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

using Grobid.PdfToXml;

namespace Grobid.NET
{
    // Extract dates from a XML document of the following form.
    //
    // <dates>
    //   <date><month>March</month> <day>28</day>, <year>2007</year></date>
    // </dates>
    //
    // An XML document may have more than one <date /> element.

    public sealed class DateFeatureFactory
    {
        public IEnumerable<TeiFeature[]> Create(XDocument doc)
        {
            foreach (var nodes in doc.XPathSelectElements("/dates/date"))
            {
                var features = new List<TeiFeature>();

                foreach (var node in nodes.DescendantNodes())
                {
                    var ele = node as XElement;
                    if (ele != null && ele.Name.LocalName != "lb")
                    {
                        features.Add(this.Create(ele));
                    }
                    else if (node is XText && node.Parent.Name == "date")
                    {
                        features.AddRange(this.Create((XText)node));
                    }
                }

                yield return features.ToArray();
            }
        }

        private TeiFeature Create(XElement element)
        {
            return TeiFeature.Create(element.Value, element.Name.LocalName, 0);
        }

        private IEnumerable<TeiFeature> Create(XText text)
        {
            return text.Value.SplitWithDelims(PdfToXml.Constants.FullPunctuation)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select((x, i) => TeiFeature.Create(x, "other", i));
        }
    }
}