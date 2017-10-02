using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using Xunit;

using Grobid.NET;
using Grobid.NET.Scoring;
using Wapiti;

namespace Grobid.Test.Tei
{
    public class TeiFeatureMergeBatchTest
    {
        //[Fact]
        [Fact(Skip = "works on my machine only")]
        [Trait("Category", "EndToEnd")]
        public void Test()
        {
            var teiFeatureMerge = new TeiFeatureMerge();
            var teiFeatureFactory = new TeiFeatureFactory();

            var mergedCorpus = Directory.EnumerateFiles(@"c:\temp\corpus\header\label", "*.header")
                .Select(x => new {
                    HeaderFullName = x,
                    TeiFullName = Path.Combine(@"c:\temp\corpus\header\tei", $"{Path.GetFileNameWithoutExtension(x)}.tei"),
                })
                .Select(x => new
                {
                    TeiFeatures = teiFeatureFactory.Create(XDocument.Load(File.OpenRead(x.TeiFullName))),
                    FeatureRows = TeiFeatureMergeBatchTest.GetFeatureRows(x.HeaderFullName),
                })
                .SelectMany(x => teiFeatureMerge.Merge(x.TeiFeatures, x.FeatureRows))
                .ToArray();

            var sb = new StringBuilder();
            var scorer = new ModelScorer();
            foreach (var x in mergedCorpus)
            {
                scorer.Eval(x.Expected, x.Actual);
            }

            var modelScoreCard = new ModelScoreCard();
            modelScoreCard.Render(scorer.Stat, sb);

            var s = sb.ToString();
            // FROM: full corpus
            //           Label Accuracy  Precision Recall         F0
            //       abstract      0.989      0.998      0.985      0.991
            //        address      0.999      0.981      0.985      0.983
            //    affiliation      0.998      0.978      0.985      0.981
            //         author      0.999      0.980      0.994      0.987
            //      copyright      1.000      0.982      0.953      0.967
            //           date      1.000      0.975      0.966      0.971
            //date-submission      1.000      0.407      0.458      0.431
            //     dedication      1.000      0.905      0.843      0.873
            //         degree      1.000      0.993      1.000      0.996
            //          email      0.999      0.977      0.989      0.983
            //        entitle      1.000      1.000      0.986      0.993
            //          grant      0.999      0.984      0.980      0.982
            //          intro      0.995      0.298      0.751      0.427
            //        keyword      0.998      0.959      0.952      0.956
            //           note      0.997      0.934      0.942      0.938
            //          phone      1.000      0.986      0.987      0.987
            //         pubnum      1.000      0.984      0.986      0.985
            //      reference      0.998      0.950      0.970      0.960
            //     submission      0.999      0.949      0.928      0.938
            //          title      0.997      0.944      0.994      0.969
            //            web      1.000      0.986      0.988      0.987

            //     MICRO AVG.      0.965      0.982      0.982      0.982
            //     MACRO AVG.      0.998      0.912      0.934      0.918
        }

        //[Fact]
        //[Trait("Category", "EndToEnd")]
        public void Test01()
        {
            var teiFeatureMerge = new TeiFeatureMerge();
            var teiFeatureFactory = new TeiFeatureFactory();

            var model = global::Wapiti.Wapiti.Load(@"content\models\header\model.wapiti");

            var mergedCorpus = Directory.EnumerateFiles(@"C:\dev\grobid-win32\grobid-trainer\resources\dataset\header\crap\headers", "*.header")
                .Select(x => new {
                    HeaderFullName = x,
                    TeiFullName = Path.Combine(@"C:\dev\grobid-win32\grobid-trainer\resources\dataset\header\crap\tei", $"{Path.GetFileNameWithoutExtension(x)}.tei"),
                })
                .Select(x => new
                {
                    TeiFeatures = teiFeatureFactory.Create(XDocument.Load(File.OpenRead(x.TeiFullName))),
                    FeatureRows = TeiFeatureMergeBatchTest.GetLabeledRows(model, x.HeaderFullName),
                })
                .SelectMany(x => teiFeatureMerge.Merge(x.TeiFeatures, x.FeatureRows))
                .ToArray();

            var sb = new StringBuilder();
            var scorer = new ModelScorer();
            foreach (var x in mergedCorpus)
            {
                scorer.Eval(x.Expected, x.Actual);
            }

            var modelScoreCard = new ModelScoreCard();
            modelScoreCard.Render(scorer.Stat, sb);

            var s = sb.ToString();
        }

        public void Test02()
        {
            var teiFeatureMerge = new TeiFeatureMerge();
            var teiFeatureFactory = new TeiFeatureFactory();

            var model = global::Wapiti.Wapiti.Load(@"content\models\date\model.wapiti");

            var mergedCorpus = Directory.EnumerateFiles(@"C:\dev\grobid-win32\grobid-trainer\resources\dataset\date\crap\headers", "*.header")
                .Select(x => new {
                    HeaderFullName = x,
                    TeiFullName = Path.Combine(@"C:\dev\grobid-win32\grobid-trainer\resources\dataset\date\crap\tei", $"{Path.GetFileNameWithoutExtension(x)}.tei"),
                })
                .Select(x => new
                {
                    TeiFeatures = teiFeatureFactory.Create(XDocument.Load(File.OpenRead(x.TeiFullName))),
                    FeatureRows = TeiFeatureMergeBatchTest.GetLabeledRows(model, x.HeaderFullName),
                })
                .SelectMany(x => teiFeatureMerge.Merge(x.TeiFeatures, x.FeatureRows))
                .ToArray();

            var sb = new StringBuilder();
            var scorer = new ModelScorer();
            foreach (var x in mergedCorpus)
            {
                scorer.Eval(x.Expected, x.Actual);
            }

            var modelScoreCard = new ModelScoreCard();
            modelScoreCard.Render(scorer.Stat, sb);

            var s = sb.ToString();

            // FROM: grobid-trainer/resources/dataset/header/evaluation
            //       Label Accuracy  Precision Recall         F0
            //   abstract      0.945      0.984      0.932      0.957
            //    address      0.997      0.946      0.972      0.959
            //affiliation      0.997      0.953      0.976      0.964
            //     author      0.999      0.989      0.994      0.991
            //  copyright      0.996      0.160      1.000      0.276
            //       date      1.000      1.000      1.000      1.000
            //     degree      1.000      1.000      1.000      1.000
            //      email      0.999      0.947      1.000      0.973
            //      grant      0.999      0.985      0.992      0.988
            //      intro      0.958      0.015      0.600      0.029
            //    keyword      0.999      0.979      0.940      0.959
            //       note      0.987      1.000      0.488      0.656
            //      phone      1.000      1.000      1.000      1.000
            //     pubnum      1.000      1.000      1.000      1.000
            // submission      1.000      1.000      1.000      1.000
            //      title      1.000      0.996      0.996      0.996
            //        web      1.000      1.000      0.950      0.974
            // MICRO AVG.      0.876      0.934      0.934      0.934
            // MACRO AVG.      0.993      0.880      0.932      0.866
        }

        private static FeatureRow[] GetFeatureRows(string fileName)
        {
            var featureRows = File.ReadAllLines(fileName)
              .Select(x => x.Trim())
              .Where(x => !String.IsNullOrWhiteSpace(x))
              .Select(FeatureRow.Parse)
              .ToArray();

            return featureRows;
        }

        private static FeatureRow[] GetLabeledRows(WapitiModel model, string filenName)
        {
            var labeled = new StringReader(model.Label(File.ReadAllLines(filenName)))
                .ReadAllLines()
                .Where(x => x != string.Empty)
                .Select(FeatureRow.Parse)
                .ToArray();

            return labeled;
        }
    }
}
