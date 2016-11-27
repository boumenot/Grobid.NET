using System.Linq;

using FluentAssertions;
using Xunit;

using Grobid.NET;

namespace Grobid.Test
{
    public sealed class ExtensionsTest
    {
        [Fact]
        public void TakeUntilIncludesMatchedValueTest()
        {
            var xs = Enumerable.Range(0, 5);
            xs.TakeUntil(x => x < 3).Should().ContainInOrder(0);
        }

        [Fact]
        public void TakeUntilTest()
        {
            var xs = Enumerable.Range(0, 5);
            xs.TakeUntil(x => x > 2).Should().ContainInOrder(0, 1, 2, 3);
        }
    }
}
