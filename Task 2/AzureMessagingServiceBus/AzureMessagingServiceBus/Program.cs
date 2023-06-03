using Azure.Messaging.ServiceBus;
using AzureMessagingServiceBus;
using Microsoft.Azure.Amqp.Framing;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace AzurePublisher
{
    internal class Program
    {
        private const string SenderID = "sender007";
        private const string ServiceBusConnectionString = "Endpoint=sb://asadservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=P54dtRtj4VZouPvISiH8XudlJhjL/NYkW+ASbDHkFYY=";
        private const string TopicName = "azuremessageservice";

        public static async Task Main()
        {
            var client = new ServiceBusClient(ServiceBusConnectionString);
            var topicClient = client.CreateSender(TopicName);

            // Create and publish messages
            //var message1 = CreateMessage(Category.ProcessImage);
            var message2 = CreateMessage(Category.ProcessVideo);
            //var message3 = CreateMessage(Category.ProcessTimeData);

            //await topicClient.SendMessageAsync(message1);
            await topicClient.SendMessageAsync(message2);
            //await topicClient.SendMessageAsync(message3);

            Console.WriteLine("Messages published.");


            Console.Read();
        }

        private static ServiceBusMessage CreateMessage(Category category)
        {
            var payload = GetPayload(category);

            MsgCls mc = new MsgCls();
            mc.MessageId = Guid.NewGuid();
            mc.SenderID = SenderID;
            mc.CreatedTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            mc.Category = category;
            mc.Payload = payload;

            AzureEntities1 db = new AzureEntities1();
            AzureMessageLog aml = new AzureMessageLog();

            aml.MessageId = mc.MessageId.ToString();
            aml.SenderId = mc.SenderID;
            aml.CreatedTime = mc.CreatedTime;
            aml.Category = mc.Category.ToString();
            aml.Payload = JsonConvert.SerializeObject(mc.Payload); 

            db.AzureMessageLogs.Add(aml);
            db.SaveChanges();

            //var payload = GetPayload(category);

            var message = new ServiceBusMessage();
            message.Body = new BinaryData(JsonConvert.SerializeObject(mc));
            //message.Body = new BinaryData(JsonConvert.SerializeObject(new
            //{
            //    MessageId = Guid.NewGuid(),
            //    SenderID,
            //    CreatedTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            //    Category = category,
            //    Payload = payload
            //}));

            Console.WriteLine(message);

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

    public class MsgCls
    {
        public Guid MessageId { get; set; } 
        public string SenderID { get; set; }
        public string CreatedTime { get; set; }
        public object Category { get; set; }
        public object Payload { get; set; }
    }
}

