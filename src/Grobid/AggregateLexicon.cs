using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grobid.NET
{
    public class AggregateLexicon : ILexicon
    {
        private readonly ILexicon[] lexicons;
        private Lexicon englishLexicon;
        private Lexicon germanLexicon;
        private org.grobid.core.lexicon.FastMatcher cityNameFastMatcher;
        private CountryCodes countryCodes;

        public AggregateLexicon(ILexicon lexicon, params ILexicon[] lexicons)
        {
            this.lexicons = Enumerable.Repeat<ILexicon>(lexicon, 1).Concat(lexicons).ToArray();
        }

        public AggregateLexicon(IEnumerable<ILexicon> lexicons)
        {
            this.lexicons = lexicons.ToArray();
        }

        public AggregateLexicon(
            Lexicon englishLexicon, 
            Lexicon germanLexicon, 
            org.grobid.core.lexicon.FastMatcher cityNameFastMatcher,
            CountryCodes countryCodes)
        {
            this.englishLexicon = englishLexicon;
            this.germanLexicon = germanLexicon;
            this.lexicons = new[] { this.englishLexicon, this.germanLexicon };

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
