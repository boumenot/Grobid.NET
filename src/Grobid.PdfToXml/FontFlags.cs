using System;

namespace Grobid.PdfToXml
{
    [Flags]
    public enum FontFlags
    {
        FixedWidth = 0x00000001,
        Serif      = 0x00000002,
        Symbolic   = 0x00000004,
        Italic     = 0x00000040,
        Bold       = 0x00040000,
    }
}