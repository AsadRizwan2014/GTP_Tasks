using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticLogCreation
{
    using Nest;
    using System;

    class Program
    {
        static void Main()
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200")).DefaultIndex("logs");
            var client = new ElasticClient(settings);
            int numberOfLogs = 10; 

            for (int i = 0; i < numberOfLogs; i++)
            {
                string logEntry = GenerateLogEntry();
                Console.WriteLine(logEntry);
                SendLogToElasticsearch(client, logEntry);
            }

            //while (true)
            //{
            //    string logEntry = GenerateLogEntry();
            //    Console.WriteLine(logEntry);
            //    SendLogToElasticsearch(client, logEntry);
            //}
        }

        static string GenerateLogEntry()
        {
            Guid guid = Guid.NewGuid(); // Generate a unique GUID
            string randomText = GenerateRandomText(); // Generate random text

            string logEntry = $"{guid}: {randomText}";

            return logEntry;
        }

        static string GenerateRandomText()
        {
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            int length = 10; // Length of random text

            // Generate random text 
            string randomText = new string(Enumerable.Repeat(characters, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return randomText;
        }
        static void SendLogToElasticsearch(ElasticClient client, string logEntry)
        {
            var logDocument = new
            {
                message = logEntry
            };

            var indexResponse = client.IndexDocument(logDocument);

            if (indexResponse.IsValid)
            {
                Console.WriteLine("Log entry sent to Elasticsearch successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to send log entry to Elasticsearch. Error: {indexResponse.ServerError}");
            }
        }

    }

}
