using System;
using System.IO;

using ApprovalTests;
using ApprovalTests.Reporters;
using FluentAssertions;
using Xunit;

using Grobid.NET;
using org.apache.log4j;
using org.grobid.core;

namespace Grobid.Test
{
    [UseReporter(typeof(DiffReporter))]
    public class GrobidTest
    {
        static GrobidTest()
        {
            // BasicConfigurator.configure();
            // org.apache.log4j.Logger.getRootLogger().setLevel(Level.DEBUG);
        }

        [Fact]
        [Trait("Test", "EndToEnd")]
        public void ExtractTest()
        {
            var binPath = Environment.GetEnvironmentVariable("PDFTOXMLEXE");

            var factory = new GrobidFactory(
                "grobid.zip",
                binPath,
                Directory.GetCurrentDirectory());

            var grobid = factory.Create();
            var result = grobid.Extract(@"essence-linq.pdf");

            result.Should().NotBeEmpty();
            Approvals.Verify(result);
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
