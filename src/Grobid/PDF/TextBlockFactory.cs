using System.Collections.Generic;
using System.Linq;

namespace Grobid.NET
{
    public class TextBlockFactory
    {
        public IEnumerable<TextBlock> Create(IReadOnlyList<TextInfo> textInfos, float pageHeight)
        {
            var i = 0;
            while (i < textInfos.Count)
            {
                var y = textInfos[i].BoundingRectangle.Bottom;

                var textBlocks = textInfos
                    .Skip(i)
                    .TakeWhile(x => x.IsEmpty || x.BoundingRectangle.Bottom == y);

                var xs = textBlocks.ToArray();
                yield return new TextBlock(xs, pageHeight);

                i += xs.Length;
            }
        }
    }
}
