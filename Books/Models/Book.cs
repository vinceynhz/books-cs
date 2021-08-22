using System.Text.Json;

namespace Models
{

    public class Book
    {
        public string Title { get; set; }
        public string Isbn { get; set; }
        public string Year { get; set; }

        public static Book ParseRawString(string isbn, string rawString)
        {
            using (JsonDocument document = JsonDocument.Parse(rawString))
            {
                JsonElement root = document.RootElement;
                if (root.TryGetProperty($"ISBN:{isbn}", out JsonElement isbnNode))
                {
                    Book result = new Book();
                    result.Isbn = isbn;

                    if (isbnNode.TryGetProperty("title", out JsonElement title))
                    {
                        result.Title = title.GetString();
                    }

                    if (isbnNode.TryGetProperty("publish_date", out JsonElement year))
                    {
                        result.Year = year.GetString();
                    }
                    return result;
                }
            }

            return null;
        }

        override
        public string ToString()
        {
            return $"Book<{this.Title} - {this.Year} ({this.Isbn})>";
        }
    }
}