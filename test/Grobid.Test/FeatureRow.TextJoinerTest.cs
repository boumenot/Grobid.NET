using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using Grobid.NET;

using Xunit;

namespace Grobid.Test
{
    public class TextJoinerTest
    {
        [Fact]
        public void Test()
        {
            var testSubject = new FeatureRowTextJoiner();
            var featureRows = new[]
            {
                new FeatureRow() { Classification = "email", Value = "sam" },
                new FeatureRow() { Classification = "email", Value = "." },
                new FeatureRow() { Classification = "email", Value = "lindley@strath" },
                new FeatureRow() { Classification = "email", Value = "." },
                new FeatureRow() { Classification = "email", Value = "ac" },
                new FeatureRow() { Classification = "email", Value = "." },
                new FeatureRow() { Classification = "email", Value = "uk" },
            };

            var email = testSubject.Join(featureRows);
            email.Should().Be("sam.lindley@strath.ac.uk");
        }
    }
}
