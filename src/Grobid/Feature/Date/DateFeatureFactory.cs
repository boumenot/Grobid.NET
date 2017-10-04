using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

using Grobid.NET.Feature;
using Grobid.NET.Feature.Date;
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
        private readonly DateFeatureVectorFactory factory;

        public DateFeatureFactory(DateFeatureVectorFactory factory)
        {
            this.factory = factory;
        }

        public IEnumerable<LabeledFeature[]> Create(XDocument doc)
        {
            foreach (var nodes in doc.XPathSelectElements("/dates/date"))
            {
                var features = new List<LabeledFeature>();

                foreach (var node in nodes.DescendantNodes())
                {
                    var ele = node as XElement;
                    if (ele != null && ele.Name.LocalName != "lb")
                    {
                        features.AddRange(
                            this.Create(ele.Value, ele.Name.LocalName));
                    }
                    else if (node is XText && node.Parent.Name == "date")
                    {
                        features.AddRange(
                            this.Create(((XText)node).Value, "other"));
                    }
                }

                this.SetLineBoundaries(features);
                yield return features.ToArray();
            }
        }

        private void SetLineBoundaries(List<LabeledFeature> features)
        {
            if (features.Count > 1)
            {
                features.First().FeatureVector.LineStatus = LineStatus.LINESTART;
                features.Last().FeatureVector.LineStatus = LineStatus.LINEEND;
            }
        }

        private IEnumerable<LabeledFeature> Create(string text, string classification)
        {
            return text
                .SplitWithDelims(PdfToXml.Constants.FullPunctuation)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select((x, i) => this.Create(x, classification, i));
        }

        private LabeledFeature Create(string text, string classification, int tokenIndex)
        {
            var featureVector = this.factory.Create(
                this.CreateBlockState(text));

            return LabeledFeature.Create(featureVector, classification, tokenIndex);
        }

        private BlockState CreateBlockState(string text)
        {
            return new BlockState
            {
                BlockStatus = BlockStatus.BLOCKIN,
                FontStatus = FontStatus.SAMEFONT,
                LineStatus = LineStatus.LINEIN,
                FontSizeStatus = FontSizeStatus.SAMEFONTSIZE,
                Text = text,
            };
        }
    }
}