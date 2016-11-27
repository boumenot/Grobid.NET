﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Grobid.NET.Model
{
    public sealed class HeaderFactory
    {
        private static readonly Regex AbstractRx = new Regex(@"\s*abstract?\s*", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex KeywordRx = new Regex(@"\s*keywords?\s*", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private readonly FeatureRowGrouper grouper = new FeatureRowGrouper();
        private readonly NoSpaceJoiner noSpaceJoiner = new NoSpaceJoiner();
        private readonly SentenceTextJoiner sentenceJoiner = new SentenceTextJoiner();

        private static readonly FeatureRow[] Empty = new FeatureRow[0];

        public Header Create(FeatureRow[] featureRows)
        {
            var model = new Header();

            var groups = this.grouper.Group(featureRows);
            this.ProcessTitle(groups, model);
            this.ProcessAuthors(groups, model);
            this.ProcessKeywords(groups, model);
            this.ProcessAbstract(groups, model);

            return model;
        }

        private FeatureRow[] GetByClassification(ArraySegment<FeatureRow>[] groups, string classification)
        {
            var rows = groups
                .FirstOrDefault(x => x.First().Classification == classification)
                .ToArray();

            return rows;
        }

        private void ProcessTitle(ArraySegment<FeatureRow>[] groups, Header model)
        {
            var rows = this.GetByClassification(groups, Constants.Classification.Title);
            model.Title = this.sentenceJoiner.Join(rows);
        }

        private void ProcessAuthors(ArraySegment<FeatureRow>[] groups, Header model)
        {
            var d = new Dictionary<string, FeatureRow[]>();

            foreach (var g in groups)
            {
                var classification = g.First().Classification;

                if (!this.IsAuthor(classification))
                {
                    continue;
                }

                if (d.ContainsKey(classification))
                {
                    this.ProcessAuthor(d, model);
                    d.Clear();
                }

                d[classification] = g.ToArray();
            }

            if (d.Any())
            {
                this.ProcessAuthor(d, model);
            }
        }

        private bool IsAuthor(string classification)
        {
            return classification == Constants.Classification.Author ||
                   classification == Constants.Classification.Affiliation ||
                   classification == Constants.Classification.Email;
        }

        private void ProcessAuthor(Dictionary<string, FeatureRow[]> authorFeatureRows, Header model)
        {
            var author = new Author();

            foreach (var classification in authorFeatureRows.Keys)
            {
                switch (classification)
                {
                    case Constants.Classification.Author:
                        author.Name = this.sentenceJoiner.Join(authorFeatureRows[classification]);
                        break;
                    case Constants.Classification.Affiliation:
                        author.Affiliation = this.sentenceJoiner.Join(authorFeatureRows[classification]);
                        break;
                    case Constants.Classification.Email:
                        author.Email = this.noSpaceJoiner.Join(authorFeatureRows[classification]);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"The classification {classification} is not supported.");
                }
            }

            model.Authors.Add(author);
        }

        private void ProcessKeywords(ArraySegment<FeatureRow>[] groups, Header model)
        {
            var rows = this.GetByClassification(groups, Constants.Classification.Keyword);

            var text = this.sentenceJoiner.Join(rows);
            text = HeaderFactory.KeywordRx.Replace(text, string.Empty);

            var keywords = text
                .Split(',')
                .Select(x => x.Trim());

            model.Keywords.AddRange(keywords);
        }

        private void ProcessAbstract(ArraySegment<FeatureRow>[] groups, Header model)
        {
            var rows = this.GetByClassification(groups, Constants.Classification.Abstract);
            var text = this.sentenceJoiner.Join(rows);
            text = HeaderFactory.AbstractRx.Replace(text, string.Empty);

            model.Abstract = text;
        }
    }
}