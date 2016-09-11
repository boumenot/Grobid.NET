using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace Grobid.Test
{
    public class FeatureVectorExtractorFactoryTest
    {
        [Fact]
        public void Test()
        {
        }
    }

    public delegate void BlockBeginHandler(object sender, EventArgs e);

    public class FeatureEvents
    {
        //public event BlockBeginHandler BlockBegin;
    }

    public class FeatureVectorExtractorFactory {
        private readonly FeatureExtractor featureExtractor;

        public FeatureVectorExtractorFactory(FeatureExtractor featureExtractor)
        {
            this.featureExtractor = featureExtractor;
        }

        public Func<string, string>[] CreateHeaderExtractor()
        {
            throw new NotImplementedException();
        }

        public Func<string, string>[] CreateAuthorExtractor()
        {
            throw new NotImplementedException();
        }

        public Func<string, string>[] CreateAffilationExtractor()
        {
            throw new NotImplementedException();
        }
    }

    public class FeatureVectorHeaderExtractor {}
}
