using System;
using System.IO;
using System.Linq;

using ApprovalTests;
using ApprovalTests.Reporters;

using Newtonsoft.Json;
using Xunit;

using Grobid.NET;
using Grobid.NET.Feature;
using Grobid.PdfToXml;
using Grobid.Test.Feature;

namespace Grobid.Test
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

            Func<HeaderFeatureVector, string[]> formatter = x =>
            {
                return new string[]
                {
                    x.Text,
                    x.AsLowerCase,
                    x.Prefix1,
                    x.Prefix2,
                    x.Prefix3,
                    x.Prefix4,
                    x.Suffix1,
                    x.Suffix2,
                    x.Suffix3,
                    x.Suffix4,
                    x.BlockStatus.ToString(),
                    x.LineStatus.ToString(),
                    x.FontStatus.ToString(),
                    x.FontSizeStatus.ToString(),
                    x.IsBold ? "1" : "0",
                    x.IsItalic ? "1" : "0",
                    x.IsRotation ? "1" : "0",
                    x.Capitalization.ToString(),
                    x.Digit.ToString(),
                    x.IsSingleChar ? "1" : "0",
                    x.IsProperName ? "1" : "0",
                    x.IsDictionaryWord ? "1" : "0",
                    x.IsFirstName ? "1" : "0",
                    x.IsLocationName ? "1" : "0",
                    x.IsEmailAddress ? "1" : "0",
                    x.IsYear ? "1" : "0",
                    x.IsMonth ? "1" : "0",
                    x.IsHttp ? "1" : "0",
                    x.HasDash ? "1" : "0",
                    x.Punctuation.ToString(),
                    "0",
                    "0",
                };
            };

            var testSubject = new PdfBlockExtractor<string[]>();

            var introductionFilter = new IntroductionFilter();
            var preIntroBlocks = blocks.TakeUntil(introductionFilter.IsIntroduction).ToArray();

            var featureVectors = testSubject.Extract(preIntroBlocks, x => formatter(headerFeatureVectorFactory.Create(x)));

            Approvals.Verify(
                String.Join(Environment.NewLine, featureVectors.Select(x => String.Join(" ", x))));
        }
    }
}
