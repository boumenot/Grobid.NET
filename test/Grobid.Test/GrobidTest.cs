using System;
using System.IO;
using System.Linq;

using ApprovalTests;
using ApprovalTests.Reporters;
using Newtonsoft.Json;
using Wapiti;
using Xunit;

using Grobid.NET;
using Grobid.NET.Entity;
using Grobid.NET.Feature;
using Grobid.NET.Feature.Header;
using Grobid.PdfToXml;

namespace Grobid.Test
{
    [UseReporter(typeof(BeyondCompare4Reporter))]
    public class GrobidTest
    {
        [Fact, Trait("Category", "EndToEnd")]
        public void Test()
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
            var lines = featureVectors
                .Select(x => String.Join(" ", x))
                .ToArray();

            using (var model = global::Wapiti.Wapiti.Load(@"content\models\header\model.wapiti"))
            {
                var labeled = new StringReader(model.Label(lines))
                    .ReadAllLines()
                    .Where(x => x != string.Empty)
                    .Select(FeatureRow.Parse)
                    .ToArray();

                Approvals.VerifyJson(JsonConvert.SerializeObject(labeled));
            }
        }

        [Fact, Trait("Category", "EndToEnd")]
        public void HeaderModel()
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
            var lines = featureVectors
                .Select(x => String.Join(" ", x))
                .ToArray();

            using (var model = global::Wapiti.Wapiti.Load(@"content\models\header\model.wapiti"))
            {
                var labeled = new StringReader(model.Label(lines))
                    .ReadAllLines()
                    .Where(x => x != string.Empty)
                    .Select(FeatureRow.Parse)
                    .ToArray();

                var modelFactory = new HeaderFactory();
                var headerModel = modelFactory.Create(labeled);

                Approvals.VerifyJson(JsonConvert.SerializeObject(headerModel));
            }
        }
    }
}
