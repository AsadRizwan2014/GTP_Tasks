using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace KafkaPublisher
{
    internal class Program
    {
        private const string BootstrapServers = "localhost:9092";
        private const string TopicName = "Test_Topic";

        public static async Task Main()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = BootstrapServers
            };

            var producer = new ProducerBuilder<string, string>(config).Build();
            
            // Create and publish messages
            var message1 = CreateMessage(Category.ProcessImage);
            var message2 = CreateMessage(Category.ProcessVideo);
            var message3 = CreateMessage(Category.ProcessTimeData);

            //await producer.ProduceAsync(TopicName, new Message<string, string> { Key = message1.MessageId, Value = JsonConvert.SerializeObject(message1) });
            await producer.ProduceAsync(TopicName, new Message<string, string> { Key = message2.MessageId, Value = JsonConvert.SerializeObject(message2) });
            //await producer.ProduceAsync(TopicName, new Message<string, string> { Key = message3.MessageId, Value = JsonConvert.SerializeObject(message3) });

            producer.Flush(TimeSpan.FromSeconds(10));

            Console.WriteLine("Messages published.");
        }

        private static KafkaMessage CreateMessage(Category category)
        {
            var payload = GetPayload(category);

            var message = new KafkaMessage
            {
                MessageId = Guid.NewGuid().ToString(),
                Category = category,
                Payload = payload,
                CreatedTime = DateTime.UtcNow,
                SenderId = "Sender007"
            };

            //KafkaEntities db = new KafkaEntities();
            //Message m = new Message();
            //m.MessageId = message.MessageId;
            //m.SenderId = message.SenderId;
            //m.CreatedTime = message.CreatedTime;
            //m.Category = message.Category.ToString();
            //m.Payload = message.Payload.ToString();

            //db.Messages.Add(m);
            //db.SaveChanges();

            return message;
        }

        private static object GetPayload(Category category)
        {
            switch (category)
            {
                case Category.ProcessImage:
                    return new ImagePayload
                    {
                        Id = Guid.NewGuid(),
                        FileName = "image.jpg",
                        FileSize = 1024,
                        Type = "JPEG"
                    };

                case Category.ProcessVideo:
                    return new VideoPayload
                    {
                        Id = Guid.NewGuid(),
                        FileName = "video.mp4",
                        FileSize = 2048,
                        Codec = "H.264"
                    };

                case Category.ProcessTimeData:
                    return new TimeDataPayload
                    {
                        Id = Guid.NewGuid(),
                        Position = 42,
                        TimeMilliseconds = 5000,
                        DataList = new TimeDataPayload[0]
                    };

                default:
                    throw new ArgumentOutOfRangeException(nameof(category));
            }
        }
    }

    public enum Category
    {
        ProcessImage,
        ProcessVideo,
        ProcessTimeData
    }

    public class Message
    {
        public string MessageId { get; set; }
        public string SenderId { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Category { get; set; }
        public string Payload { get; set; }
    }

    public class KafkaMessage
    {
        public string MessageId { get; set; }
        public Category Category { get; set; }
        public object Payload { get; set; }
        public DateTime CreatedTime { get; set; }
        public string SenderId { get; set; }
    }

    public class ImagePayload
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public string Type { get; set; }
    }

    public class VideoPayload
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public string Codec { get; set; }
    }

    public class TimeDataPayload
    {
        public Guid Id { get; set; }
        public int Position { get; set; }
        public int TimeMilliseconds { get; set; }
        public TimeDataPayload[] DataList { get; set; }
    }
}
