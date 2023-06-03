using Nest;
using System;

namespace ESearch
{
    internal class Program
    {
        static void Main()
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
                .DefaultIndex("logs");

            var client = new ElasticClient(settings);

            Console.Write("Enter the text to search:\n");
            var a = Console.ReadLine();

            // Search log data
            var searchResponse = client.Search<LogEntry>(s => s
            .Query(q => q
                .MultiMatch(m => m
                    .Fields(f => f
                        .Field(ff => ff.Id)
                        .Field(ff => ff.Message)
                    )
                    .Query(a)
                )
            )
        );

            if (searchResponse.IsValid)
            {
                Console.WriteLine($"Total log entries found: {searchResponse.Total}");

                foreach (var hit in searchResponse.Hits)
                {
                    Console.WriteLine($"Log entry ID: {hit.Id}");
                    Console.WriteLine($"Log entry text: {hit.Source.Message}");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine($"Failed to search log entries: {searchResponse.ServerError?.Error}");
            }
        }
    }

    class LogEntry
    {
        public string Id { get; set; }
        public string Message { get; set; }
    }

}
