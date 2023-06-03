using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace RabbitMQPublisher
{
    internal class Program
    {
        private const string RabbitMQConnectionString = "amqp://guest:guest@localhost:5672/";
        private const string ExchangeName = "RabbitExchage";

        public static void Main()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {

                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                const string message = rabbitmqmessage;
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: string.Empty,
                                     routingKey: "hello",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine($" [x] Sent {message}");

                Console.WriteLine(" Press [enter] to exit.");
                //Console.ReadLine();
                Thread.Sleep(1000);

            }

        }

        private static rabbitmqmessage createmessage(category category)
        {
            var payload = getpayload(category);

            var message = new rabbitmqmessage
            {
                category = category,
                payload = payload,
                createdtime = DateTime.UtcNow,
                senderid = "your_sender_id"
            };

            return  message;
        }

        private static object getpayload(category category)
        {
            switch (category)
            {
                case category.processimage:
                    return new imagepayload
                    {
                        id = Guid.NewGuid(),
                        filename = "image.jpg",
                        filesize = 1024,
                        type = "jpeg"
                    };

                case category.processvideo:
                    return new videopayload
                    {
                        id = Guid.NewGuid(),
                        filename = "video.mp4",
                        filesize = 2048,
                        codec = "h.264"
                    };

                case category.processtimedata:
                    return new timedatapayload
                    {
                        id = Guid.NewGuid(),
                        position = 42,
                        timemilliseconds = 5000,
                        datalist = new timedatapayload[0]
                    };

                default:
                    throw new ArgumentOutOfRangeException(nameof(category));
            }
        }
    }

    public enum category
    {
        processimage,
        processvideo,
        processtimedata
    }

    public class imagepayload
    {
        public Guid id { get; set; }
        public string filename { get; set; }
        public int filesize { get; set; }
        public string type { get; set; }
    }

    public class videopayload
    {
        public Guid id { get; set; }
        public string filename { get; set; }
        public int filesize { get; set; }
        public string codec { get; set; }
    }

    public class timedatapayload
    {
        public Guid id { get; set; }
        public int position { get; set; }
        public int timemilliseconds { get; set; }
        public timedatapayload[] datalist { get; set; }
    }

    public class rabbitmqmessage
    {
        public category category { get; set; }
        public object payload { get; set; }
        public DateTime createdtime { get; set; }
        public string senderid { get; set; }
    }

}

