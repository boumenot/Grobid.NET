using com.sun.tools.javap;
using java.util;
using javax.naming;
using org.apache.log4j;
using org.grobid.core.document;
using org.grobid.core.engines;
using org.grobid.core.lexicon;
using org.grobid.core.main;
using org.grobid.core.mock;
using org.grobid.core.process;
using org.grobid.core.utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Grobid.NET
{
    public class DummyContext : javax.naming.Context
    {
        public object addToEnvironment(string propName, object propVal)
        {
            throw new NotImplementedException();
        }

        public void bind(string name, object obj)
        {
            throw new NotImplementedException();
        }

        public void bind(javax.naming.Name name, object obj)
        {
            throw new NotImplementedException();
        }

        public void close()
        {
            throw new NotImplementedException();
        }

        public string composeName(string name, string prefix)
        {
            throw new NotImplementedException();
        }

        public javax.naming.Name composeName(javax.naming.Name name, javax.naming.Name prefix)
        {
            throw new NotImplementedException();
        }

        public javax.naming.Context createSubcontext(string name)
        {
            throw new NotImplementedException();
        }

        public javax.naming.Context createSubcontext(javax.naming.Name name)
        {
            throw new NotImplementedException();
        }

        public void destroySubcontext(string name)
        {
            throw new NotImplementedException();
        }

        public void destroySubcontext(javax.naming.Name name)
        {
            throw new NotImplementedException();
        }

        public java.util.Hashtable getEnvironment()
        {
            throw new NotImplementedException();
        }

        public string getNameInNamespace()
        {
            throw new NotImplementedException();
        }

        public javax.naming.NameParser getNameParser(string name)
        {
            throw new NotImplementedException();
        }

        public javax.naming.NameParser getNameParser(javax.naming.Name name)
        {
            throw new NotImplementedException();
        }

        public javax.naming.NamingEnumeration list(string name)
        {
            throw new NotImplementedException();
        }

        public javax.naming.NamingEnumeration list(javax.naming.Name name)
        {
            throw new NotImplementedException();
        }

        public javax.naming.NamingEnumeration listBindings(string name)
        {
            throw new NotImplementedException();
        }

        public javax.naming.NamingEnumeration listBindings(javax.naming.Name name)
        {
            throw new NotImplementedException();
        }

        public object lookup(string name)
        {
            throw new NotImplementedException();
        }

        public object lookup(javax.naming.Name name)
        {
            throw new NotImplementedException();
        }

        public object lookupLink(string name)
        {
            throw new NotImplementedException();
        }

        public object lookupLink(javax.naming.Name name)
        {
            throw new NotImplementedException();
        }

        public void rebind(string name, object obj)
        {
            throw new NotImplementedException();
        }

        public void rebind(javax.naming.Name name, object obj)
        {
            throw new NotImplementedException();
        }

        public object removeFromEnvironment(string propName)
        {
            throw new NotImplementedException();
        }

        public void rename(string oldName, string newName)
        {
            throw new NotImplementedException();
        }

        public void rename(javax.naming.Name oldName, javax.naming.Name newName)
        {
            throw new NotImplementedException();
        }

        public void unbind(string name)
        {
            throw new NotImplementedException();
        }

        public void unbind(javax.naming.Name name)
        {
            throw new NotImplementedException();
        }
    }

    public class GrobidFactory
    {
        private readonly string pathToModelsZip;
        private readonly string pathToPdf2XmlExe;
        private readonly string pathToTemp;

        //private static GrobidProperties grobidProperties;
        private static LexiconFactory lexiconFactory = new LexiconFactory();

        static GrobidFactory()
        {
            GrobidFactory.SetPathForNativeDllResolution();

            //BasicConfigurator.configure();
            //org.apache.log4j.Logger.getRootLogger().setLevel(Level.DEBUG);

        //    var assemblyPath = Assembly.GetExecutingAssembly().Location;
        //    var assemblyDirectory = Path.GetDirectoryName(assemblyPath);

        //    var libwapiti_dll = Path.Combine(assemblyDirectory, Constants.Native.libwapiti_dll);
        //    LibraryLoader.explicitLoad(libwapiti_dll);

        //    var libwapiti_swig_dll = Path.Combine(assemblyDirectory, Constants.Native.libwapiti_swig_dll);
        //    LibraryLoader.explicitLoad(libwapiti_swig_dll);
        }

        public GrobidFactory(
            string pathToModelsZip,
            string pathToPdf2XmlExe,
            string pathToTemp)
        {
            //GrobidProperties.set_GROBID_HOME_PATH("c:\\temp\\grobid\\home");
            //GrobidProperties.setGrobidPropertiesPath("c:\\temp\\grobid\\props");

            GrobidProperties.setContext(new InitialContext());
            GrobidProperties.set_GROBID_HOME_PATH("c:\\");
            GrobidProperties.setGrobidPropertiesPath("c:\\");

            //grobidProperties = GrobidProperties.getInstance();
            
            var assemblyPath = Assembly.GetExecutingAssembly().Location;
            var assemblyDirectory = Path.GetDirectoryName(assemblyPath);

            //var libwapiti_dll = Path.Combine(assemblyDirectory, Constants.Native.libwapiti_dll);
            var libwapiti_dll = @"C:\dev\Grobid.NET\lib\native\x64\libwapiti.dll";
            LibraryLoader.explicitLoad(libwapiti_dll);

            //var libwapiti_swig_dll = Path.Combine(assemblyDirectory, Constants.Native.libwapiti_swig_dll);
            var libwapiti_swig_dll = @"C:\dev\Grobid.NET\lib\native\x64\libwapiti_swig.dll";
            LibraryLoader.explicitLoad(libwapiti_swig_dll);

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

        private static void SetPathForNativeDllResolution()
        {
            if (IntPtr.Size != 8)
            {
                const string message = "Grobid.NET only support 64-bit processes!";
                throw new ArgumentOutOfRangeException(message);
            }

            var assemblyFullName = new Uri(Assembly.GetExecutingAssembly().EscapedCodeBase).LocalPath;
            var assemblyPath = Path.GetDirectoryName(assemblyFullName);
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
