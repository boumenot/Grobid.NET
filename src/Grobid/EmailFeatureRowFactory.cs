using System;
using System.Linq;

namespace Grobid.NET
{
    public class EmailFeatureRowFactory {
        public string Parse(FeatureRow[] featureRows)
        {
            return String.Join(string.Empty, featureRows.Select(x => x.Value));
        }
    }
}