using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using Xunit;

namespace Grobid.Test
{
    public class Wapiti
    {
        public const string DllName = @"C:\dev\wapiti.git\build\libwapiti.dll";

        public delegate string gets_cb(IntPtr input);
        public delegate int print_cb(IntPtr output, string format, params object[] args);

        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern IntPtr iol_new2(
            [MarshalAs(UnmanagedType.FunctionPtr)]gets_cb gets,
            IntPtr input,
            [MarshalAs(UnmanagedType.FunctionPtr)]print_cb print,
            IntPtr output);

        [DllImport(DllName)]
        public static extern IntPtr rdr_new(
            IntPtr iol,
            [MarshalAs(UnmanagedType.I1)]bool autouni);

        [DllImport(DllName)]
        public static extern IntPtr mdl_new(IntPtr rdr);

        [DllImport(DllName)]
        public static extern void mdl_load(IntPtr mdl);

        [DllImport(DllName)]
        public static extern void tag_label(IntPtr mdl, IntPtr iol);
    }

    public class WapitiNativeMethodsTest
    {
        [Fact]
        public void TestA()
        {
            int i = 0;
            var lines = File.ReadAllLines(@"c:/temp/grobid/home/models/name/header/model.wapiti", Encoding.ASCII);

            Wapiti.gets_cb gets_cb1 = x =>
            {
                if (i >= lines.Length)
                    return null;

                var line = lines[i];
                i += 1;

                return line;
            };

            GC.KeepAlive(gets_cb1);

            var iol = Wapiti.iol_new2(
                gets_cb1,
                IntPtr.Zero,
                null,
                IntPtr.Zero);

            var rdr = Wapiti.rdr_new(iol, true);
            var mdl = Wapiti.mdl_new(rdr);

            Wapiti.mdl_load(mdl);

            i = 0;
            var lines2 = File.ReadAllLines(@"c:/dev/author_header.txt", Encoding.ASCII);
            Wapiti.gets_cb gets_cb2 = x =>
            {
                if (i >= lines2.Length)
                    return null;

                var line = lines2[i];
                i += 1;

                return line;
            };

            GC.KeepAlive(gets_cb2);

            var iol2 = Wapiti.iol_new2(
                gets_cb2,
                IntPtr.Zero,
                null,
                IntPtr.Zero);

            Wapiti.tag_label(mdl, iol2);
            Console.WriteLine("blah");
        }
    }
}
