using System;
using System.Linq;

using Grobid.NET.Contract;

namespace Grobid.NET
{
    public sealed class FeatureRowTextJoiner : IFeatureRowStringJoiner
    {
        public string Join(FeatureRow[] featureRows)
        {
            return String.Join(string.Empty, featureRows.Select(x => x.Value));
        }
    }
}