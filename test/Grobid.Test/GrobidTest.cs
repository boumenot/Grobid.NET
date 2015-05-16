using org.grobid.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Grobid.Test
{
    public class GrobidTest
    {
        [Fact]
        public void Test()
        {
            var x = GrobidModels.NAMES_HEADER;
            x.toString().Should().Be("name/header");
        }
    }
}
