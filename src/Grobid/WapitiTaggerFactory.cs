using System;
using System.IO.Compression;
using System.Linq;

using fr.limsi.wapiti;
using org.grobid.core.engines.tagging;
using org.grobid.core.jni;

namespace Grobid.NET
{
    internal sealed class WapitiTaggerFactory : TaggerFactory, IDisposable
    {
        private readonly ZipArchive archive;

        public WapitiTaggerFactory(
            ZipArchive archive)
        {
            this.archive = archive;
        }

        public GenericTagger create(org.grobid.core.GrobidModels gm)
        {
            var fullname = $"models/{gm.toString()}/model.wapiti";
            var entry = this.archive.Entries.FirstOrDefault(x => x.FullName == fullname);

            if (entry == null)
            {
                var message = $"The models archive does not contain a model for '{fullname}'!";
                throw new NullReferenceException(message);
            }

            var io = new GrobidWapitiIO(
                new ikvm.io.InputStreamWrapper(entry.Open()),
                null);

            var model = new WapitiModel(io);
            return new WapitiTagger(model);
        }

        public void Dispose()
        {
            using (this.archive) { }
        }
    }
}
