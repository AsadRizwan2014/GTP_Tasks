using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitSubscriber
{
    internal class Program
    {
        private const string RabbitMQConnectionString = "amqp://guest:guest@localhost:5672/";
        private const string QueueName = "hello";

        public static void Main()
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(RabbitMQConnectionString)
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (sender, args) =>
                {
                    var body = args.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var payload = JsonConvert.DeserializeObject<object>(message);

                    Console.WriteLine($"Received message: {message}");

                    channel.BasicAck(args.DeliveryTag, false);
                };

                channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);

                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            }
        }
    }
}
