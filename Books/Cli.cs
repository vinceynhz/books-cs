using System;
using System.Threading.Tasks;
using CommandLine;

namespace Books
{
    enum ReturnCode
    {
        OK = 0,
        WRONG_PARAMETER = -1,
        DATA_ERROR = -2,
        NO_RESULT = -3,
    }

    [Verb("search", HelpText = "Seach for a book in the open library API or in the local library")]
    class SearchOptions
    {
        [Option("local", HelpText = "Search in local library")]
        public bool Local { get; set; }

        [Option('i', "isbn", SetName = "isbn", HelpText = "ISBN of the book to search")]
        public string Isbn { get; set; }

        [Option('t', "title", SetName = "title", HelpText = "Title of the book to search")]
        public string Title { get; set; }

        public async Task<int> Process()
        {
            ReturnCode rc = ReturnCode.OK;
            Models.Book result = null;
            if (this.Local)
            {
                Console.WriteLine("Searching in local library...");
            }
            else
            {
                Console.WriteLine("Searching in open library...");
                if (null != this.Isbn && this.Isbn.Length > 0)
                {
                    result = await Services.OpenLibraryService.AsyncSearchByIsbn(this.Isbn);
                }
                else if (null != this.Title && this.Title.Length > 0)
                {
                    Models.SearchResults results = await Services.OpenLibraryService.AsyncSearchByTitle(this.Title);
                    if (null != results)
                    {
                        for (int i = 0; i < results.Count; i++)
                        {
                            Console.WriteLine(results.Entries[i].ToString());
                        }
                    }
                }
            }

            if (result == null)
            {
                rc = ReturnCode.DATA_ERROR;
            }
            else
            {
                Console.WriteLine(result.ToString());
            }
            return (int)rc;
        }

    }

    [Verb("none", HelpText = "Nothing")]
    class Other
    { }
    public class Runner
    {
        public Task<int> run(string[] args)
        {
            return CommandLine.Parser.Default.ParseArguments<SearchOptions, Other>(args)
            .MapResult(
                (SearchOptions opts) => opts.Process(),
                (Other opts) => Task.FromResult((int)ReturnCode.OK),
                errors => Task.FromResult((int)ReturnCode.WRONG_PARAMETER)
            );
        }
    }
}