using System;
using System.IO;
using System.Linq;
using ApprovalTests;
using ApprovalTests.Reporters;
using Grobid.NET;
using Grobid.NET.Feature;
using Grobid.NET.Feature.Header;
using Grobid.PdfToXml;
using Newtonsoft.Json;
using Xunit;

namespace Grobid.Test.Feature.Header
{
    [UseReporter(typeof(DiffReporter))]
    public class HeaderFeatureVectorTest
    {
        [Fact]
        public void HeaderFeatureExtractionTest()
        {
            var testInfoFactory = new PageBlockFactory();
            var pageBlocks = testInfoFactory.Create(Sample.Pdf.OpenEssenseLinq(), 1);

            var blocks = pageBlocks.SelectMany(x => x.Blocks);

            var featureExtractor = new FeatureExtractor(EmptyLexicon.Instance);
            var headerFeatureVectorFactory = new HeaderFeatureVectorFactory(featureExtractor);

            var testSubject = new PdfBlockExtractor<HeaderFeatureVector>();
            var featureVectors = testSubject.Extract(blocks, x => headerFeatureVectorFactory.Create(x));

            Approvals.VerifyJson(JsonConvert.SerializeObject(featureVectors));
        }

        /// <summary>
        /// Create output that is suitable for use with Wapiti.
        /// </summary>
        [Fact]
        public void HeaderFeatureExtractionWapitiTest()
        {
            var testInfoFactory = new PageBlockFactory();
            var pageBlocks = testInfoFactory.Create(Sample.Pdf.OpenEssenseLinq(), 1);

            var blocks = pageBlocks.SelectMany(x => x.Blocks);
            var reflower = new BlockReflower();
            blocks = reflower.Reflow(blocks);

            var lexiconFactory = new LexiconFactory();
            var englishLexicon = lexiconFactory.Create(
                File.OpenRead(@"content\lexicon\names\firstnames.txt"),
                File.OpenRead(@"content\lexicon\names\lastnames.txt"),
                File.OpenRead(@"content\lexicon\wordforms\english-normalized.wf"));

            var germanLexicon = lexiconFactory.Create(
                File.OpenRead(@"content\lexicon\wordforms\german-normalized.wf"));

            var lexicon = new AggregateLexicon(englishLexicon, germanLexicon);

            var featureExtractor = new FeatureExtractor(lexicon);
            var headerFeatureVectorFactory = new HeaderFeatureVectorFactory(featureExtractor);

            var testSubject = new PdfBlockExtractor<string[]>();

            var introductionFilter = new IntroductionFilter();
            var preIntroBlocks = blocks.TakeUntil(introductionFilter.IsIntroduction).ToArray();

            var featureVectors = testSubject.Extract(
                preIntroBlocks, 
                x => HeaderFeatureFormatter.Instance.Format(headerFeatureVectorFactory.Create(x)));

            Approvals.Verify(
                String.Join(Environment.NewLine, featureVectors.Select(x => String.Join(" ", x))));
        }
    }
}
