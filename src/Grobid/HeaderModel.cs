using System.Collections.Generic;

namespace Grobid.NET
{
    public class HeaderModel
    {
        public HeaderModel()
        {
            this.Authors = new List<AuthorModel>();
            this.Keywords = new List<string>();
        }

        public string Title { get; set; }
        public string Abstract { get; set; }
        public List<AuthorModel> Authors { get; }
        public List<string> Keywords { get; }
    }
}