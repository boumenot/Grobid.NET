using System.Collections.Generic;

using FluentAssertions;
using Xunit;

using Grobid.NET;
using Grobid.PdfToXml;

namespace Grobid.Test
{
    public class IntroductionFilterTest
    {
        [Fact]
        public void Test()
        {
            var testSubject = new IntroudctionFilter();
            this.CreateFakeBlocks("1. PROBLEM").TakeUntil(testSubject.IsIntroduction).Should().HaveCount(8);
            this.CreateFakeBlocks("1. PROBLEMS").TakeUntil(testSubject.IsIntroduction).Should().HaveCount(8);

            this.CreateFakeBlocks("1. Introduction").TakeUntil(testSubject.IsIntroduction).Should().HaveCount(8);
            this.CreateFakeBlocks("1.\nIntroduction").TakeUntil(testSubject.IsIntroduction).Should().HaveCount(8);

            this.CreateFakeBlocks("1. Content").TakeUntil(testSubject.IsIntroduction).Should().HaveCount(8);
            this.CreateFakeBlocks("1.\nContent").TakeUntil(testSubject.IsIntroduction).Should().HaveCount(8);

            this.CreateFakeBlocks("1. INTRODUCTION").TakeUntil(testSubject.IsIntroduction).Should().HaveCount(8);

            this.CreateFakeBlocks("I. Introduction").TakeUntil(testSubject.IsIntroduction).Should().HaveCount(8);
            this.CreateFakeBlocks("I.  Introduction").TakeUntil(testSubject.IsIntroduction).Should().HaveCount(8);

            this.CreateFakeBlocks("I. Einleitung").TakeUntil(testSubject.IsIntroduction).Should().HaveCount(8);

            this.CreateFakeBlocks("1. Einleitung").TakeUntil(testSubject.IsIntroduction).Should().HaveCount(8);

            this.CreateFakeBlocks("1 Einleitung").TakeUntil(testSubject.IsIntroduction).Should().HaveCount(8);

            this.CreateFakeBlocks("1 Introduction").TakeUntil(testSubject.IsIntroduction).Should().HaveCount(8);

            this.CreateFakeBlocks("Will Not Match").TakeUntil(testSubject.IsIntroduction).Should().HaveCount(15);
            this.CreateFakeBlocks("2. Introduction").TakeUntil(testSubject.IsIntroduction).Should().HaveCount(15);
        }

        private Block[] CreateFakeBlocks(string text)
        {
            var blocks = new List<Block>();

            for (int i = 0; i < 7; i++)
            {
                var tokenBlock = new TokenBlock { Text = "Something" };
                var textBlock = new TextBlock(new[] { tokenBlock }, -1);

                blocks.Add(new Block { TextBlocks = new [] { textBlock }});

            }

            var blockToBeMatched = new Block
            {
                TextBlocks = new[]
                {
                    new TextBlock(
                        new[]
                        {
                            new TokenBlock
                            {
                                Text = text
                            }
                        }),
                }
            };

            blocks.Add(blockToBeMatched);

            for (int i = 0; i < 7; i++)
            {
                var tokenBlock = new TokenBlock { Text = "Something" };
                var textBlock = new TextBlock(new[] { tokenBlock }, -1);

                blocks.Add(new Block { TextBlocks = new[] { textBlock } });
            }

            return blocks.ToArray();
        }
    }

    public class IntroudctionFilter
    {
        public bool IsIntroduction(Block arg)
        {
            return DocumentStructure.IntroductionStrict.IsMatch(arg.Text);
        }
    }
}
