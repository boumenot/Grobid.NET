using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using Grobid.NET;

using Xunit;

namespace Grobid.Test
{
    public class EmailFeatureRowFactoryTest
    {
        [Fact]
        public void Test()
        {
            var testSubject = new EmailFeatureRowFactory();
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

            var email = testSubject.Parse(featureRows);
            email.Should().Be("sam.lindley@strath.ac.uk");
        }
    }
}
