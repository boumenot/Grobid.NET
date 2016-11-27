using System.Text;

using Grobid.NET.Contract;

namespace Grobid.NET
{
    public sealed class SentenceTextJoiner : IFeatureRowStringJoiner
    {
        public string Join(FeatureRow[] featureRows)
        {
            var sb = new StringBuilder();
            var state = SentenceTextState.Space;

            foreach (var featureRow in featureRows)
            {
                if (sb.Length > 0 && this.AbutsWord(featureRow.Value))
                {
                    sb.Replace(' ', featureRow.Value[0], sb.Length - 1, 1);
                }
                else if (sb.Length > 0 && this.IsWordJoiner(featureRow))
                {
                    sb.Replace(' ', featureRow.Value[0], sb.Length - 1, 1);
                    state = SentenceTextState.NoSpace;
                }
                else if (state == SentenceTextState.NoSpace)
                {
                    sb.Remove(sb.Length - 1, 1); // trim extraneous whitespace
                    sb.Append(featureRow.Value);
                    state = SentenceTextState.Space;
                }
                else
                {
                    sb.Append(featureRow.Value);
                }

                sb.Append(" ");
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1); // trim extraneous whitespace
            }

            return sb.ToString();
        }

        private bool IsWordJoiner(FeatureRow featureRow)
        {
            switch (featureRow.Value)
            {
                case "-":
                // Not strictly correct, but let's see how sufficient it is.
                //  -> PASS: The cat's meow.
                //  -> FAIL: I like to 'quote' a quote.
                //
                // It is very difficult to determine where to put a quote.  Should it
                // be attached to previous or next word?
                case "'":
                    return true;
                default:
                    return false;
            }
        }

        private bool AbutsWord(string value)
        {
            switch (value)
            {
                case ".":
                case "?":
                case "!":
                case ",":
                case ":":
                case ";":
                    return true;
                default:
                    return false;
            }
        }
    }
}