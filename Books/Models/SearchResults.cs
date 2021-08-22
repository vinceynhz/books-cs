using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Models
{
    public class SearchResults
    {
        [JsonPropertyName("numFound")]
        public int Count { get; set; }
        [JsonPropertyName("docs")]
        public List<Entries> Entries { get; set; }

    }

    public class Entries
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("author_name")]
        public List<string> Authors { get; set; }

        [JsonPropertyName("isbn")]
        public List<string> Isbns { get; set; }

        override
        public string ToString()
        {
            int count = this.Authors != null ? this.Authors.Count : 0;
            int isbns = this.Isbns != null ? this.Isbns.Count : 0;
            return $"Entry<{this.Title} - {count} ({isbns})";
        }
    }
}