using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;

using javax.naming;
using org.grobid.core.document;
using org.grobid.core.engines;
using org.grobid.core.lexicon;
using org.grobid.core.main;
using org.grobid.core.process;
using org.grobid.core.utilities;

namespace Grobid.NET
{
    public class GrobidFactory
    {
        private readonly string pathToModelsZip;
        private readonly string pathToPdf2XmlExe;
        private readonly string pathToTemp;

        private static LexiconFactory lexiconFactory = new LexiconFactory();

        static GrobidFactory()
        {
            GrobidFactory.SetPathForNativeDllResolution();

            var libwapiti_dll = GrobidFactory.GetFullNativeWapitiName("libwapiti.dll");
            LibraryLoader.explicitLoad(libwapiti_dll);

            var libwapiti_swig_dll = GrobidFactory.GetFullNativeWapitiName("libwapiti_swig.dll");
            LibraryLoader.explicitLoad(libwapiti_swig_dll);

            //BasicConfigurator.configure();
            //org.apache.log4j.Logger.getRootLogger().setLevel(Level.DEBUG);
        }

        public GrobidFactory(
            string pathToModelsZip,
            string pathToPdf2XmlExe,
            string pathToTemp)
        {
            GrobidProperties.setContext(new InitialContext());
            GrobidProperties.set_GROBID_HOME_PATH("c:\\");
            GrobidProperties.setGrobidPropertiesPath("c:\\");

            var assemblyPath = Assembly.GetExecutingAssembly().Location;
            var assemblyDirectory = Path.GetDirectoryName(assemblyPath);

            this.pathToModelsZip = pathToModelsZip;
            this.pathToPdf2XmlExe = pathToPdf2XmlExe;
            this.pathToTemp = pathToTemp;
        }

        public IGrobid Create()
        {
            var pdfFactory = new PdfToXmlConverterImpl(
                new PdfToXmlCmdFactory(new java.io.File(this.pathToPdf2XmlExe)),
                new java.io.File(this.pathToTemp));

            var archive = new ZipArchive(File.OpenRead(this.pathToModelsZip));

            var lexicon = this.CreateLexicons(archive);
            var featureTester = new FeatureTesterImpl(lexicon);

            var documentFactory = new DocumentFactory(
                new PdfXmlParser(),
                featureTester,
                lexicon);

            var taggerFactory = new WapitiTaggerFactory(
                archive);

            var engine = EngineParsers.Create(
                pdfFactory,
                documentFactory,
                taggerFactory,
                featureTester,
                lexicon);

            return new GrobidEngine(engine, lexicon, taggerFactory);
        }

        private static string GetExecutingAssemblyPath()
        {
            var assemblyFullName = new Uri(Assembly.GetExecutingAssembly().EscapedCodeBase).LocalPath;
            var assemblyPath = Path.GetDirectoryName(assemblyFullName);

            return assemblyPath;
        }

        private static string GetFullNativeWapitiName(string file)
        {
            var assemblyPath = GrobidFactory.GetExecutingAssemblyPath();
            var path = Path.Combine(assemblyPath, "native", "x64", file);

            return path;
        }

        private static void SetPathForNativeDllResolution()
        {
            if (IntPtr.Size != 8)
            {
                const string message = "Grobid.NET only support 64-bit processes!";
                throw new ArgumentOutOfRangeException(message);
            }

            var assemblyPath = GrobidFactory.GetExecutingAssemblyPath();
            var nativeDllPath = Path.Combine(assemblyPath, "NativeBinaries", "x64", "lib");

            const string PATH = "PATH";

            var newPath = string.Format("{0};{1}",
                nativeDllPath,
                Environment.GetEnvironmentVariable(PATH));

            Environment.SetEnvironmentVariable(PATH, newPath);
        }

        private ILexicon CreateLexicons(ZipArchive archive)
        {
            var englishLexicon = GrobidFactory.lexiconFactory.Create(
                archive.Entries.First(x => x.FullName == "lexicon/names/firstnames.txt").Open(),
                archive.Entries.First(x => x.FullName == "lexicon/names/lastnames.txt").Open(),
                archive.Entries.First(x => x.FullName == this.GetWordformPath("english")).Open());

            var germanLexicon = GrobidFactory.lexiconFactory.Create(
                archive.Entries.First(x => x.FullName == this.GetWordformPath("german")).Open());

            var cityNameFastMatcher = new FastMatcher(
                this.ReadAllLines(archive.Entries.First(x => x.FullName == "lexicon/places/cities15000.txt").Open()));

            var countryCodes = CountryCodes.FromTei(
                archive.Entries.First(x => x.FullName == "lexicon/countries/CountryCodes.xml").Open());

            var lexicon = new AggregateLexicon(englishLexicon, germanLexicon, cityNameFastMatcher, countryCodes);
            return lexicon;
        }

        private string GetWordformPath(string name)
        {
            return String.Format("lexicon/wordforms/{0}-normalized.wf", name);
        }

        private string[] ReadAllLines(Stream stream)
        {
            var lines = new List<string>();

            using (var reader = new StreamReader(stream))
            {
                while (true)
                {
                    var line = reader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }

                    lines.Add(line);
                }
            }

            return lines.ToArray();
        }
    }
}
