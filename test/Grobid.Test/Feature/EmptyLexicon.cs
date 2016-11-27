using System;

using Grobid.NET.Contract;

namespace Grobid.Test.Feature
{
    public class EmptyLexicon : ILexicon
    {
        public static EmptyLexicon Instance = new EmptyLexicon();

        private EmptyLexicon()
        {
        }

        public bool IsFirstName(string name)
        {
            return StringComparer.OrdinalIgnoreCase.Compare(name, "christopher") == 0;
        }

        public bool IsLastName(string name)
        {
            return StringComparer.OrdinalIgnoreCase.Compare(name, "boumenot") == 0;
        }

        public bool HasWord(string word)
        {
            return StringComparer.OrdinalIgnoreCase.Compare(word, "word") == 0;
        }
    }
}
