﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace Grobid.NET
{
    public class PageBackFactory
    {
        private readonly int maxPagesToRead;

        public PageBackFactory() : this(int.MaxValue) {}

        public PageBackFactory(int maxPagesToRead)
        {
            this.maxPagesToRead = maxPagesToRead;
        }

        public PageBlock[] Create(Stream stream)
        {
            using (var reader = new PdfReader(stream))
            {
                return this.ReadPages(reader).ToArray();
            }
        }

        private IEnumerable<PageBlock> ReadPages(PdfReader reader)
        {
            int pagesToRead = Math.Min(reader.NumberOfPages, this.maxPagesToRead);

            for (int i = 1; i <= pagesToRead; i++)
            {
                var textInfos = this.GetTextInfos(reader, i);

                var pageBlock = this.CreatePageBlock(
                    reader.GetPageSize(i).Width,
                    reader.GetPageSize(i).Height,
                    textInfos);

                yield return pageBlock;
            }
        }

        private PageBlock CreatePageBlock(float width, float height, List<TokenBlock> textInfos)
        {
            var pageBlock = new PageBlock
            {
                Width = width,
                Height = height,
                TextInfos = textInfos,
            };
            return pageBlock;
        }

        private List<TokenBlock> GetTextInfos(PdfReader reader, int pageNumber)
        {
            var textInfos = new List<TokenBlock>();
            var xmlTextExtractionStrategy = new XmlTextExtractionStrategy(textInfos);

            PdfTextExtractor.GetTextFromPage(reader, pageNumber, xmlTextExtractionStrategy);
            return textInfos;
        }
    }
}
