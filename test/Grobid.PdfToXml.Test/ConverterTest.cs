using System.Text;
using System.IO;
using System.Linq;

using ApprovalTests;
using ApprovalTests.Reporters;

using Xunit;


namespace Grobid.PdfToXml.Test
{
    [UseReporter(typeof(DiffReporter))]
    public class ConverterTest
    {
        [Fact]
        public void EssenseShouldSpecificTokenBlocks()
        {
            var testSubject = new Converter();

            var pageBackFactory = new PageBlockFactory();
            var pageBlocks = pageBackFactory.Create(Sample.Pdf.OpenEssenseLinq(), 1);

            var doc = testSubject.ToXml(pageBlocks);
            Approvals.VerifyXml(doc.ToString());
        }

        // [Fact]
        public void TestThingy()
        {
            var pageBackFactory = new PageBlockFactory();
            var pageBlocks = pageBackFactory.Create(Sample.Pdf.OpenEssenseLinq(), 1);

            var sb = new StringBuilder();
            int page = 0;

            foreach (var pageBlock in pageBlocks)
            {
                var textBlocks = pageBlock.Blocks.SelectMany(x => x.TextBlocks).ToArray();
                for (int i = 0; i < textBlocks.Length; i++)
                {
                    var textBlock = textBlocks[i];

                    sb.AppendFormat($"block[{i}]:");
                    sb.AppendFormat($" font={textBlock.TokenBlocks[0].FontName.FullName}");
                    sb.AppendFormat($", fontSize={textBlock.TokenBlocks[0].FontSize}");
                    sb.AppendFormat($", color={textBlock.TokenBlocks[0].FontColor}");
                    sb.AppendFormat($", isBold={textBlock.TokenBlocks[0].IsBold}");
                    sb.AppendFormat($", isItalic={textBlock.TokenBlocks[0].IsItalic}");
                    sb.AppendFormat($", height={textBlock.Height}");
                    sb.AppendFormat($", width={textBlock.Width}");
                    sb.AppendFormat($", X={textBlock.X}");
                    sb.AppendFormat($", Y={textBlock.Y}");
                    sb.AppendFormat($", nbTokens={textBlock.TokenBlocks.Length}");
                    sb.AppendFormat($", page={page}");
                    sb.AppendFormat($", text='''{textBlock.Text}'''");
                    sb.AppendLine();

                    for (int j=0; j < textBlock.TokenBlocks.Length; j++)
                    {
                        var tokenBlock = textBlock.TokenBlocks[j];
                        sb.AppendFormat($"  layout[{j}]:");
                        sb.AppendFormat($" font={tokenBlock.FontName.FullName}");
                        sb.AppendFormat($", fontSize={tokenBlock.FontSize}");
                        sb.AppendFormat($", color={tokenBlock.FontColor}");
                        sb.AppendFormat($", isBold={tokenBlock.IsBold}");
                        sb.AppendFormat($", isItalic={tokenBlock.IsItalic}");
                        sb.AppendFormat($", height={tokenBlock.Height}");
                        sb.AppendFormat($", width={tokenBlock.Width}");
                        sb.AppendFormat($", X={tokenBlock.X}");
                        sb.AppendFormat($", Y={tokenBlock.Y}");
                        sb.AppendFormat($", rotation={tokenBlock.Rotation}");
                        sb.AppendFormat($", text='''{tokenBlock.Text}'''");
                        sb.AppendLine();
                    }
                }

                page++;
            }

            File.WriteAllText(@"c:\temp\grobid-dot-net.log", sb.ToString());
        }

        // [Fact]
        public void TestThingy01()
        {
            var pageBackFactory = new PageBlockFactory();
            var pageBlocks = pageBackFactory.Create(Sample.Pdf.OpenEssenseLinq(), 1);

            var sb = new StringBuilder();
            int page = 0;

            foreach (var pageBlock in pageBlocks)
            {
                var textBlocks = pageBlock.Blocks.SelectMany(x => x.TextBlocks).ToArray();
                for (int i = 0; i < textBlocks.Length; i++)
                {
                    var textBlock = textBlocks[i];

                    sb.AppendFormat($"block[{i}]:");
                    sb.AppendFormat($" font={textBlock.TokenBlocks[0].FontName.FullName}");
                    sb.AppendFormat($", fontSize={textBlock.TokenBlocks[0].FontSize}");
                    sb.AppendFormat($", color={textBlock.TokenBlocks[0].FontColor}");
                    sb.AppendFormat($", isBold={textBlock.TokenBlocks[0].IsBold}");
                    sb.AppendFormat($", isItalic={textBlock.TokenBlocks[0].IsItalic}");
                    sb.AppendFormat($", height={textBlock.Height}");
                    sb.AppendFormat($", width={textBlock.Width}");
                    sb.AppendFormat($", X={textBlock.X}");
                    sb.AppendFormat($", Y={textBlock.Y}");
                    sb.AppendFormat($", nbTokens={textBlock.TokenBlocks.Length}");
                    sb.AppendFormat($", page={page}");
                    sb.AppendFormat($", text='''{textBlock.Text}'''");
                    sb.AppendLine();

                    for (int j = 0; j < textBlock.TokenBlocks.Length; j++)
                    {
                        var tokenBlock = textBlock.TokenBlocks[j];
                        sb.AppendFormat($"  layout[{j}]:");
                        sb.AppendFormat($" font={tokenBlock.FontName.FullName}");
                        sb.AppendFormat($", fontSize={tokenBlock.FontSize}");
                        sb.AppendFormat($", color={tokenBlock.FontColor}");
                        sb.AppendFormat($", isBold={tokenBlock.IsBold}");
                        sb.AppendFormat($", isItalic={tokenBlock.IsItalic}");
                        sb.AppendFormat($", height={tokenBlock.Height}");
                        sb.AppendFormat($", width={tokenBlock.Width}");
                        sb.AppendFormat($", X={tokenBlock.X}");
                        sb.AppendFormat($", Y={tokenBlock.Y}");
                        sb.AppendFormat($", rotation={tokenBlock.Rotation}");
                        sb.AppendFormat($", text='''{tokenBlock.Text}'''");
                        sb.AppendLine();
                    }
                }

                page++;
            }

            File.WriteAllText(@"c:\temp\grobid-dot-net-02.log", sb.ToString());
        }
    }
}
