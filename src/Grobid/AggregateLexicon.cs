using System.Collections.Generic;
using System.Linq;

namespace Grobid.NET
{
    public class AggregateLexicon : ILexicon
    {
        private readonly ILexicon[] lexicons;
        private readonly CountryCodes countryCodes;

        public AggregateLexicon(ILexicon lexicon, params ILexicon[] lexicons)
        {
            this.lexicons = Enumerable.Repeat(lexicon, 1).Concat(lexicons).ToArray();
        }

        public AggregateLexicon(IEnumerable<ILexicon> lexicons)
        {
            this.lexicons = lexicons.ToArray();
        }

        public AggregateLexicon(
            ILexicon englishLexicon,
            ILexicon germanLexicon,
            CountryCodes countryCodes)
        {
            this.lexicons = new ILexicon[] { englishLexicon, germanLexicon };

            this.countryCodes = countryCodes;
        }

        public bool IsFirstName(string name)
        {
            return this.lexicons.Any(x => x.IsFirstName(name));
        }

        public bool IsLastName(string name)
        {
            return this.lexicons.Any(x => x.IsLastName(name));
        }

        public bool HasWord(string word)
        {
            return this.lexicons.Any(x => x.HasWord(word));
        }
    }
}
