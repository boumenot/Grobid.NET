using System.Linq;

using FluentAssertions;
using Xunit;

using Grobid.NET;

namespace Grobid.Test
{
    public class TokenAlignerTest
    {
        [Fact]
        public void TokenAligner00()
        {
            var source = new[] { "abc", "def" };
            var target = new[] { "abc", "def" };

            this.Align(source, target).Should().BeEquivalentTo("abc", "def");
        }

        [Fact]
        public void TokenAligner01()
        {
            var source = new[] { "abc", "ghi" };
            var target = new[] { "abc", "def" };

            this.Align(source, target).Should().BeEquivalentTo("abc");
        }

        [Fact]
        public void TokenAligner02()
        {
            var source = new[] { "def", "ghi" };
            var target = new[] { "abc", "def" };

            this.Align(source, target).Should().BeEquivalentTo("def");
        }

        [Fact]
        public void TokenAligner03()
        {
            var source = new[] { "abc", "def", "ghi", "jkl", "mno", "pqrs", "tuv", "wxyz" };
            var target = new[] { "cat", "dog", "bird", "deer", "bison", "lion", "gazelle" };

            this.Align(source, target).Should().BeEmpty();
        }

        [Fact]
        public void TokenAligner04()
        {
            var source = new[] { "abc", "def", "ghi", "jkl", "mno", "pqrs", "tuv", "wxyz" };
            var target = new[] { "abc", "ghi", "jkl", "mno", "pqrs", "tuv", "wxyz" };

            this.Align(source, target).Should().BeEquivalentTo(target);
        }

        [Fact]
        public void TokenAligner05()
        {
            var source = new[] { "abc", "def", "ghi", "jkl", "mno", "pqrs", "tuv", "wxyz" };
            var target = new[] { "abc", "wxyz" };

            this.Align(source, target).Should().BeEquivalentTo("abc", "wxyz");
            this.Align(target, source).Should().BeEquivalentTo(target);
        }

        [Fact]
        public void TokenAligner06()
        {
            var source = new[] { "3", "4", "5", "7", "15" };
            var target = new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" };

            this.Align(source, target).Should().BeEquivalentTo("3", "4", "5", "7");
        }

        [Fact]
        public void TokenAligner07()
        {
            var source1 = new[] { "1", "2" };
            var target1 = new[] { "1", ".", ".", "2" };

            this.Align(source1, target1, 3).Should().BeEquivalentTo("1", "2");
            this.Align(source1, target1, 2).Should().BeEquivalentTo("1");
            this.Align(source1, target1, 1).Should().BeEquivalentTo("1");
            this.Align(source1, target1, 0).Should().BeEquivalentTo();
        }

        private string[] Align(string[] source, string[] target, int windowSize = TokenAligner<string>.DefaultWindowSize)
        {
            var testSubject = new TokenAligner<string>(windowSize);
            var results = testSubject.Align(
                source,
                x => x,
                target,
                x => x,
                (src, tgt, x) => x);

            return results.ToArray();
        }
    }
}
