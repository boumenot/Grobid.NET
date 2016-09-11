using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Grobid
{
    public class FeatureExtractor
    {
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

        public string Prefix(string s, int length)
        {
            return s.Substring(0, Math.Min(s.Length, length));
        }

        public string Suffix(string s, int length)
        {
            int len = Math.Min(s.Length, length);
            int offset = s.Length - len;

            return s.Substring(offset, len);
        }

        public Capitalization Case(string s)
        {
            if (s.Length == 0)
            {
                return Capitalization.NOCAPS;
            }

            var cap = Capitalization.NOCAPS;
            if (Char.IsUpper(s[0]))
            {
                cap = Capitalization.INITCAP;
            }

            if (s.All(Char.IsUpper))
            {
                cap = Capitalization.ALLCAP;
            }

            return cap;
        }

        public Digit Digit(string s)
        {
            var digitCount = s.Count(Char.IsDigit);
            return digitCount == 0
                       ? Grobid.Digit.NODIGIT
                       : digitCount == s.Length
                           ? Grobid.Digit.ALLDIGIT
                           : Grobid.Digit.CONTAINDIGIT;
        }

        public bool IsSingleChar(string s)
        {
            return s.Length == 1;
        }

        public bool IsMonth(string s)
        {
            return FeatureExtractor.Months.Contains(s);
        }

        public bool IsYear(string s)
        {
            return s.Length == 4 &&
                   s.All(Char.IsDigit) &&
                   (s[0] == '1' || s[0] == '2');
        }

        public bool IsEmailAddress(string s)
        {
            return FeatureExtractor.EmailAddress.IsMatch(s);
        }
    }
}