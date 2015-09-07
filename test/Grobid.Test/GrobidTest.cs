using org.grobid.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Grobid.NET;
using System.Xml.Linq;
using org.apache.log4j;

namespace Grobid.Test
{
    public class GrobidTest
    {
        static GrobidTest()
        {
            BasicConfigurator.configure();
            org.apache.log4j.Logger.getRootLogger().setLevel(Level.DEBUG);
        }

        [Fact]
        public void ExtractTest()
        {
            var factory = new GrobidFactory(
                @"c:\dev\grobid.net\grobid.zip",
                @"c:\dev\grobid.net\bin\pdf2xml.exe",
                @"c:\temp");

            var grobid = factory.Create();
            var result = grobid.Extract(@"c:\dev\grobid.net\content\essence-linq.pdf");

            result.Should().NotBeEmpty();

            Action test = () => XDocument.Parse(result);
            test.ShouldNotThrow();
        }

        [Fact]
        public void Test()
        {
            var x = GrobidModels.NAMES_HEADER;
            x.name().Should().Be("NAMES_HEADER");
            x.toString().Should().Be("name/header");
        }
    }
}
