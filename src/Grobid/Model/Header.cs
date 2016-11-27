using System.Collections.Generic;

namespace Grobid.NET.Model
{
    public class Header
    {
        public Header()
        {
            this.Authors = new List<Author>();
            this.Keywords = new List<string>();
        }

        public string Title { get; set; }
        public string Abstract { get; set; }
        public List<Author> Authors { get; }
        public List<string> Keywords { get; }
    }
}