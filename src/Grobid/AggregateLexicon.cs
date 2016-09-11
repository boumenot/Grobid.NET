using System.Collections.Generic;
using System.Linq;

namespace Grobid.NET
{
    public class AggregateLexicon : ILexicon
    {
        private readonly ILexicon[] lexicons;
        private readonly org.grobid.core.lexicon.FastMatcher cityNameFastMatcher;
        private readonly CountryCodes countryCodes;

        public AggregateLexicon(ILexicon lexicon, params ILexicon[] lexicons)
        {
            this.lexicons = Enumerable.Repeat<ILexicon>(lexicon, 1).Concat(lexicons).ToArray();
        }

        public AggregateLexicon(IEnumerable<ILexicon> lexicons)
        {
            this.lexicons = lexicons.ToArray();
        }

        public AggregateLexicon(
            ILexicon englishLexicon,
            ILexicon germanLexicon,
            org.grobid.core.lexicon.FastMatcher cityNameFastMatcher,
            CountryCodes countryCodes)
        {
            this.lexicons = new ILexicon[] { englishLexicon, germanLexicon };

            this.cityNameFastMatcher = cityNameFastMatcher;
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

        public bool inDictionary(string word)
        {
            return this.HasWord(word);
        }

        public java.util.List inCityNames(java.util.List tokens)
        {
            return this.cityNameFastMatcher.matcher(tokens);
        }

        public java.util.List inCityNames(string text)
        {
            return this.cityNameFastMatcher.matcher(text);
        }

        public string getcountryCode(string country)
        {
            return this.countryCodes.GetCode(country);
        }
    }
}
