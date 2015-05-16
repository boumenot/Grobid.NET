using org.grobid.core.data;
using org.grobid.core.engines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grobid.NET
{
    internal class GrobidEngine : IGrobid, IDisposable
    {
        private readonly EngineParsers engine;
        private readonly WapitiTaggerFactory taggerFactory;

        internal GrobidEngine(
            EngineParsers engine,
            WapitiTaggerFactory taggerFactory)
        {
            this.engine = engine;
            this.taggerFactory = taggerFactory;
        }

        public string Extract(string fileName)
        {
            var parser = this.engine.getHeaderParser();

            var biblioItem = new BiblioItem();
            var result = (string)parser.processing2(fileName, false, biblioItem).getLeft();

            return result;
        }

        public void Dispose()
        {
            using (this.taggerFactory) { }
        }
    }
}
