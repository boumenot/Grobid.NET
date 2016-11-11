using System;
using System.IO;

namespace Wapiti
{
    public class WapitiModel : IDisposable
    {
        private WapitiModel(IntPtr model)
        {
            this.Model = model;
        }

        internal IntPtr Model { get; private set; }

        public static WapitiModel Load(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                return WapitiModel.Load(stream);
            }
        }

        public static WapitiModel Load(Stream stream)
        {
            var lines = new StreamReader(stream).ReadAllLines();
            return WapitiModel.Load(lines);
        }

        public static WapitiModel Load(string[] lines)
        {
            int i = 0;
            WapitiNative.gets_cb gets_cb = () => i >= lines.Length ? null : lines[i++];

            var iol = WapitiNative.iol_new3(gets_cb, null);
            var rdr = WapitiNative.rdr_new(iol, true);
            var mdl = WapitiNative.mdl_new(rdr);

            WapitiNative.mdl_load(mdl);
            return new WapitiModel(mdl);
        }

        public void Dispose()
        {
            if (this.Model != IntPtr.Zero)
            {
                WapitiNative.mdl_free(this.Model);
                this.Model = IntPtr.Zero;
            }
        }
    }
}