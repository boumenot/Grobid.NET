using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using Grobid.NET;

using Xunit;

namespace Grobid.Test
{
    public class ExtensionsTest
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
