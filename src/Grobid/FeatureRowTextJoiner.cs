using System;
using System.Linq;

namespace Grobid.NET
{
    public class FeatureRowTextJoiner
    {
        public string Join(FeatureRow[] featureRows)
        {
            return String.Join(string.Empty, featureRows.Select(x => x.Value));
        }
    }
}