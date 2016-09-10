using System;

using org.grobid.core.data;
using org.grobid.core.engines;

namespace Grobid.NET
{
    internal class GrobidEngine : IGrobid, IDisposable
    {
        private readonly EngineParsers engine;
        private readonly ILexicon lexicon;
        private readonly WapitiTaggerFactory taggerFactory;

        internal GrobidEngine(
            EngineParsers engine,
            ILexicon lexicon,
            WapitiTaggerFactory taggerFactory)
        {
            this.engine = engine;
            this.lexicon = lexicon;
            this.taggerFactory = taggerFactory;
        }

        public string Extract(string fileName)
        {
            var parser = this.engine.getHeaderParser();

            var biblioItem = new BiblioItem(this.lexicon);
            var result = (string)parser.processing2(fileName, false, biblioItem).getLeft();

            return result;
        }

        public void Dispose()
        {
            using (this.taggerFactory) { }
        }
    }
}
