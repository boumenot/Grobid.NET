using org.grobid.core.document;
using org.grobid.core.engines;
using org.grobid.core.main;
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
    public class GrobidFactory
    {
        private readonly string pathToModelsZip;
        private readonly string pathToPdf2XmlExe;
        private readonly string pathToTemp;

        private static GrobidProperties grobidProperties;

        static GrobidFactory()
        {

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
            GrobidProperties.set_GROBID_HOME_PATH("c:\\temp\\grobid\\home");
            GrobidProperties.setGrobidPropertiesPath("c:\\temp\\grobid\\props");
            grobidProperties = GrobidProperties.getInstance();
            
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

            var documentFactory = new DocumentFactory(
                new PdfXmlParser());

            var taggerFactory = new WapitiTaggerFactory(
                new ZipArchive(File.OpenRead(this.pathToModelsZip)));

            var engine = EngineParsers.Create(
                pdfFactory,
                documentFactory,
                taggerFactory);

            return new GrobidEngine(engine, taggerFactory);
        }
    }
}
