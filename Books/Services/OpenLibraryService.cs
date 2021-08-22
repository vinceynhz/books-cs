using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json;

namespace Services
{
    public static class OpenLibraryService
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task<Models.Book> AsyncSearchByIsbn(string isbn)
        {
            string baseUrl = "https://openlibrary.org";
            string apiBooksPath = "/api/books?format=json&jscmd=data";

            string url = baseUrl + apiBooksPath + "&bibkeys=ISBN:" + isbn;

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var stringTask = client.GetStringAsync(url);
                var result = Models.Book.ParseRawString(isbn, await stringTask);
                return result;
            }
            catch (HttpRequestException exception)
            {
                System.Console.WriteLine($"Error executing request: {exception.Message}");
                System.Console.WriteLine($"Requested URL: {url}");
            }
            catch (TaskCanceledException exception)
            {
                System.Console.WriteLine($"Timeout executing request: {exception.Message}");
                System.Console.WriteLine($"Requested URL: {url}");
            }
            return null;
        }

        public static async Task<Models.SearchResults> AsyncSearchByTitle(string title)
        {
            string baseUrl = "https://openlibrary.org";
            string apiSearchPath = "/search.json";

            string url = baseUrl + apiSearchPath + "?title=" + title.Replace(' ', '+');

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var streamTask = client.GetStreamAsync(url);
                var result = await JsonSerializer.DeserializeAsync<Models.SearchResults>(await streamTask);
                return result;
            }
            catch (HttpRequestException exception)
            {
                System.Console.WriteLine($"Error executing request: {exception.Message}");
                System.Console.WriteLine($"Requested URL: {url}");
            }
            catch (TaskCanceledException exception)
            {
                System.Console.WriteLine($"Timeout executing request: {exception.Message}");
                System.Console.WriteLine($"Requested URL: {url}");
            }
            return null;
        }
    }

}