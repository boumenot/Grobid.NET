using System.Text.RegularExpressions;

namespace Grobid.NET
{
    public static class DocumentStructure
    {
        /// <summary>
        /// Regular expression used to find the introduction in a document.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is similar to the regular expression found in GROBID, but it has been
        /// simplified.  One difference is the allowance of a space after 1 or I (\s*) and
        /// before the optional dot (\.?).  Due to the way PDF documents are broken down
        /// into blocks the exact text cannot be reproduced.  The outer block's text is
        /// determined by joining the inner block's text values with an assumed blank
        /// space.  It is not always correct to assume a blank.
        /// </para>
        /// <para>
        /// XXX: consider re-visiting this.
        /// </para>
        /// </remarks>
        public static Regex IntroductionStrict = new Regex(
            @"^\b[1I]\s*\.?\n?\s*(?:problems?|introduction|content|einleitung)",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }
}
