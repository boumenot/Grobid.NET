using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Grobid.NET.Feature
{
    public sealed class CountryCodes
    {
        private readonly Dictionary<string, string> countryCodes;

        private CountryCodes(Dictionary<string, string> countryCodes)
        {
            this.countryCodes = countryCodes;
        }

        public string GetCode(string country)
        {
            return this.countryCodes.GetValueOrDefault(country, null);
        }

        public static CountryCodes FromTei(Stream stream)
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            using (var reader = XmlReader.Create(stream))
            {
                var doc = XDocument.Load(reader);

                var ns = new XmlNamespaceManager(reader.NameTable);
                ns.AddNamespace("tei", "http://www.tei-c.org/ns/1.0");

                var rows = doc.XPathSelectElements("//tei:row", ns);
                foreach (var row in rows)
                {
                    var a3code = row.XPathSelectElement("tei:cell[@role='a2code']", ns).Value;
                    var countryNames = row.XPathSelectElements("tei:cell[starts-with(@role, 'name')]", ns)
                        .Select(y => y.Value.ToUpper())
                        .Distinct();

                    foreach (var countryName in countryNames)
                    {
                        dict.Add(countryName, a3code);
                    }
                }
            }

            return new CountryCodes(dict);
        }
    }
}
