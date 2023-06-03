using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaSubscriber
{
    internal class Program
    {
        private const string BootstrapServers = "Localhost:9092";
        private const string TopicName = "Test_Topic";

        public static void Main()
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = BootstrapServers,
                GroupId = "Subscribers",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(TopicName);

            var cancellationTokenSource = new CancellationTokenSource();

            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                cancellationTokenSource.Cancel();
            };

            try
            {
                while (true)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(cancellationTokenSource.Token);
                        var message = consumeResult.Message;

                        // Process the message
                        Console.WriteLine($"Received Message: {message.Value}");
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error occurred: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Consumer has been stopped
                consumer.Close();
            }
        }
    }
}
