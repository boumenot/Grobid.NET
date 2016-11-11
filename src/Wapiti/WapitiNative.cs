using System;
using System.Runtime.InteropServices;

namespace Wapiti
{
    internal class WapitiNative
    {
        public const string DllName = @"libwapiti.dll";

        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]
        public delegate string gets_cb();

        public delegate void write_cb(IntPtr ptr, int size);

        [DllImport(DllName)]
        public static extern void xfree(IntPtr ptr);

        [DllImport(DllName)]
        public static extern IntPtr iol_new3(
            [MarshalAs(UnmanagedType.FunctionPtr)] gets_cb gets,
            [MarshalAs(UnmanagedType.FunctionPtr)] write_cb write);

        [DllImport(DllName)]
        public static extern void iol_free(IntPtr iol);

        [DllImport(DllName)]
        public static extern IntPtr rdr_new(
            IntPtr iol,
            [MarshalAs(UnmanagedType.I1)] bool autouni);

        [DllImport(DllName)]
        public static extern void rdr_free(IntPtr rdr);

        [DllImport(DllName)]
        public static extern IntPtr mdl_new(IntPtr rdr);

        [DllImport(DllName)]
        public static extern void mdl_load(IntPtr mdl);

        [DllImport(DllName)]
        public static extern void mdl_free(IntPtr mdl);

        [DllImport(DllName)]
        public static extern void tag_label(IntPtr mdl, IntPtr iol);
    }
}
