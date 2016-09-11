using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using org.grobid.core.features;

namespace Grobid.NET
{
    public class FeatureTesterImpl : FeatureTester
    {
        private static readonly Regex HasPunctuation = new Regex("^[\\,\\:;\\?\\.]+$", RegexOptions.Compiled);
        private static readonly Regex EmailAddress = new Regex("^(?:[a-zA-Z0-9_'^&amp;/+-])+(?:\\.(?:[a-zA-Z0-9_'^&amp;/+-])+)*@(?:(?:\\[?(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))\\.){3}(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\]?)|(?:[a-zA-Z0-9-]+\\.)+(?:[a-zA-Z]){2,}\\.?)$", RegexOptions.Compiled);

        private static readonly HashSet<string> Months = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December",
            "Jan",
            "Feb",
            "Mar",
            "Apr",
            "Jun",
            "Jul",
            "Aug",
            "Sep",
            "Oct",
            "Nov",
            "Dec",
        };

        private readonly ILexicon lexicon;

        public FeatureTesterImpl(ILexicon lexicon)
        {
            this.lexicon = lexicon;
        }

        /// <summary>
        /// True if the strings contains no lower case letters.
        /// </summary>
        public bool test_all_capital(string str)
        {
            return !str.Any(Char.IsLower);
        }

        public bool test_common(string str)
        {
            return this.lexicon.HasWord(str);
        }

        /// <summary>
        /// True if the string contains at least one digit.
        /// </summary>
        public bool test_digit(string str)
        {
            return str.Any(Char.IsDigit);
        }

        /// <summary>
        /// True if the string is an email address.
        /// </summary>
        public bool test_email(string str)
        {
            return FeatureTesterImpl.EmailAddress.IsMatch(str);
        }

        /// <summary>
        /// True if the string contains http.
        /// </summary>
        public bool test_http(string str)
        {
            return str.Contains("http");
        }

        /// <summary>
        /// True if the string is the name of a month or an abbreviation of a month.
        /// Case is ignored.
        /// </summary>
        public bool test_month(string str)
        {
            return FeatureTesterImpl.Months.Contains(str);
        }

        public bool test_names(string str)
        {
            return this.lexicon.IsFirstName(str) || this.lexicon.IsLastName(str);
        }

        /// <summary>
        /// True if all characters in a string  are a digit.
        /// </summary>
        public bool test_number(string str)
        {
            return str.All(Char.IsDigit);
        }

        /// <summary>
        /// True if the string is composed of one or more characters of ',', '.', ':', ';', or '?'.
        /// </summary>
        public bool test_punct(string str)
        {
            return FeatureTesterImpl.HasPunctuation.IsMatch(str);
        }

        /// <summary>
        /// True if the string is a year between [1000, 2999].
        /// </summary>
        public bool test_year(string str)
        {
            return
                str.Length == 4 &&
                this.test_number(str) &&
                (str[0] == '1' || str[0] == '2');
        }

        /// <summary>
        /// True if the string is a first name.
        /// </summary>
        public bool test_first_names(string str)
        {
            return this.lexicon.IsFirstName(str);
        }

        /// <summary>
        /// True if the string is a surname.
        /// </summary>
        public bool test_last_names(string str)
        {
            return this.lexicon.IsLastName(str);
        }

        /// <summary>
        /// True if the string begins with a capital letter.
        /// </summary>
        public bool test_first_capital(string str)
        {
            return str.Length > 0 && char.IsUpper(str[0]); // && str.Skip(1).All(x => char.IsLower(x));
        }

        /// <summary>
        /// True if the string is a country.
        /// </summary>
        public bool test_country(string str)
        {
            return false;
        }
    }
}
