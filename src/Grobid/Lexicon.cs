using System;
using System.Collections.Generic;

namespace Grobid.NET
{
    public class Lexicon : ILexicon
    {
        private readonly HashSet<string> firstNames;
        private readonly HashSet<string> lastNames;
        private readonly HashSet<string> words;

        public Lexicon(
            HashSet<string> words) : this(new HashSet<string>(), new HashSet<string>(), words)
        {
        }

        public Lexicon(
            HashSet<string> firstNames,
            HashSet<string> lastNames,
            HashSet<string> words)
        {
            this.firstNames = firstNames;
            this.lastNames = lastNames;
            this.words = words;
        }

        public bool IsFirstName(string name) { return this.firstNames.Contains(name); }
        public bool IsLastName(string name) { return this.lastNames.Contains(name); }
        public bool HasWord(string word)
        {
            var normalized = word.TrimEnd('.', ',', ':', ';');

            var hyphen = normalized.Split(new[] { '-' }, 2);
            if (hyphen.Length == 2)
            {
                return
                    this.words.Contains(hyphen[0]) &&
                    this.words.Contains(hyphen[1]);
            }

            var spaced = normalized.Split(new[] { ' ' }, 2);
            if (spaced.Length == 2)
            {
                return
                    this.words.Contains(spaced[0]) &&
                    this.words.Contains(spaced[1]);
            }

            return this.words.Contains(normalized);
        }

        public bool inDictionary(string word)
        {
            return this.HasWord(word);
        }

        public java.util.List inCityNames(java.util.List tokens)
        {
            // XXX: this is wrong, Liskov is weeping.
            // Maybe this class should really be called language?
            throw new NotImplementedException();
        }

        public java.util.List inCityNames(string text)
        {
            // XXX: this is wrong, Liskov is weeping.
            throw new NotImplementedException();
        }

        public string getcountryCode(string str)
        {
            // FIXME:
            return "CT";
        }
    }
}
