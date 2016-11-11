using System;
using System.IO;
using System.Linq;

using Xunit;

namespace Wapiti.Test
{
    public class WapitiTest
    {
        [Fact]
        [Trait("Test", "EndToEnd")]
        public void Test()
        {
            //var model = WapitiModel.Load(@"c:/temp/grobid/home/models/name/header/model.wapiti");
            using (var model = WapitiModel.Load(@"c:\dev\grobid-win32\grobid-home\models\header\model.wapiti"))
            {
                var wapiti = Wapiti.Create();

                var result = Directory.EnumerateFiles(@"c:\temp\corpus\header\headers\", "*.header")
                    .Select(x => wapiti.Label(model, x))
                    .ToArray();

                var s = String.Join("\n", result);
            }
        }
    }
}
