using org.grobid.core.lexicon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grobid.NET
{
    public interface ILexicon : org.grobid.core.lexicon.Lexicon
    {
        bool IsFirstName(string name);
        bool IsLastName(string name);
        bool HasWord(string name);
    }
}
