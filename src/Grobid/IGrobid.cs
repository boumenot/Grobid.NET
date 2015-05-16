using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grobid.NET
{
    public interface IGrobid
    {
        string Extract(string fileName);
    }
}
