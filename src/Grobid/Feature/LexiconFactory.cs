using System;
using System.Collections.Generic;
using System.IO;

namespace Grobid.NET.Feature
{
    public class LexiconFactory
    {
        public Lexicon Create(
            Stream firstNames,
            Stream lastNames,
            Stream words)
        {
            var lexicon = new Lexicon(
                this.ReadLines(firstNames),
                this.ReadLines(lastNames),
                this.ReadLines(words));

            return lexicon;
        }

        public Lexicon Create(
           Stream words)
        {
            var lexicon = new Lexicon(
                this.ReadLines(words));

            return lexicon;
        }

        private HashSet<string> ReadLines(Stream stream)
        {
            var hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            using (var reader = new StreamReader(stream))
            {
                while (true)
                {
                    var line = reader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }

                    hashSet.Add(line);
                }
            }

            return hashSet;
        }
    }
}
