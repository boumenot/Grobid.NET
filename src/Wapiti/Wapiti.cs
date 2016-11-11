using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Wapiti
{
    public class Wapiti
    {
        private Wapiti() {}

        public string Label(WapitiModel model, string path)
        {
            using (var stream = File.OpenRead(path))
            {
                return this.Label(model, stream);
            }
        }

        public string Label(WapitiModel model, Stream stream)
        {
            var lines = new StreamReader(stream).ReadAllLines();
            return this.Label(model, lines);
        }

        public string Label(WapitiModel model, string[] lines)
        {
            int i = 0;
            WapitiNative.gets_cb gets_cb = () => i >= lines.Length ? null : lines[i++];

            var sb = new StringBuilder();
            WapitiNative.write_cb write_cb = (ptr, length) =>
            {
                byte[] strbuf = new byte[length];
                Marshal.Copy(ptr, strbuf, 0, length);
                var data = Encoding.UTF8.GetString(strbuf);

                WapitiNative.xfree(ptr);
                sb.Append(data);
            };

            var iol = WapitiNative.iol_new3(gets_cb, write_cb);
            WapitiNative.tag_label(model.Model, iol);
            WapitiNative.iol_free(iol);

            return sb.ToString();
        }

        public static Wapiti Create()
        {
            return new Wapiti();
        }
    }
}
