using System;
using System.Collections.Generic;

namespace Grobid.NET
{
    /// <summary>
    /// Align two streams of tokens.
    /// </summary>
    /// <typeparam name="TItem">Type of token to align two streams based on.</typeparam>
    /// <remarks>
    /// <para>
    /// Given a collection of type T, and another collection of type U we want
    /// to align the two streams based on a common type between the two, type 
    /// V.  We attempt to align based on a user specified window size.  If a
    /// matching token cannot be found within the window, we advance the 
    /// collection of type T by one and repeat.  Alignment continues until the
    /// token stream is exhausted.
    /// </para>
    /// <para>
    /// When the token streams are aligned, a transform of the T, U, and V is
    /// computed and returned.
    /// </para>
    /// <para>
    /// All of this is really fancy and confusing way to say we want to match
    /// two sets of strings, and return a new string.  There is too much variation
    /// in what the strings are hence the need for all of the genericity.
    /// </para>
    /// </remarks>
    public class TokenAligner<TItem>
        where TItem : IEquatable<TItem>
    {
        public const int DefaultWindowSize = 7;
        private readonly int windowSize;

        public TokenAligner(int windowSize = TokenAligner<TItem>.DefaultWindowSize)
        {
            this.windowSize = windowSize;
        }

        public IEnumerable<TResult> Align<TSource1, TSource2, TResult>(
            IReadOnlyList<TSource1> xs, Func<TSource1, TItem> getX,
            IReadOnlyList<TSource2> ys, Func<TSource2, TItem> getY,
            Func<TSource1, TSource2, TItem, TResult> transform)
        {
            // Index of the last matched token for the source stream.
            int lastIndex = 0;
            for (int i = 0; i < xs.Count; i++)
            {
                var tokenX = getX(xs[i]);

                for (int windowOffset = lastIndex;
                     windowOffset < ys.Count && windowOffset < lastIndex + this.windowSize;
                     windowOffset++)
                {
                    var tokenY = getY(ys[windowOffset]);
                    if (tokenX.Equals(tokenY))
                    {
                        yield return transform(xs[i], ys[windowOffset], tokenX);
                        lastIndex = windowOffset + 1;
                        break;
                    }
                }
            }
        }
    }
}