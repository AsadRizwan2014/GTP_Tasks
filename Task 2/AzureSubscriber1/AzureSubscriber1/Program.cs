using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace AzureSubscriber
{
    internal class Program
    {
        private const string ServiceBusConnectionString = "Endpoint=sb://asadservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=P54dtRtj4VZouPvISiH8XudlJhjL/NYkW+ASbDHkFYY=";
        // For Subscriber 1
        private const string SubscriptionName = "Subscriber1";
        
        // For Subscriber 2
        //private const string SubscriptionName = "Subscriber2";

        public static async Task Main()
        {
            var client = new ServiceBusClient(ServiceBusConnectionString);
            var processor = client.CreateProcessor("azuremessageservice", SubscriptionName);

            processor.ProcessMessageAsync += ProcessMessageHandler;
            processor.ProcessErrorAsync += ErrorHandler;

            await processor.StartProcessingAsync();

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

            await processor.StopProcessingAsync();
        }

        private static async Task ProcessMessageHandler(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = message.Body.ToString();
            var messageId = message.MessageId;

            // Log the unique identity of the message in the table
            Console.WriteLine($"Received message: Id = {messageId}");
            Console.WriteLine($"Received message: Body = {body}");

            await args.CompleteMessageAsync(message);
        }

        private static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }

}
