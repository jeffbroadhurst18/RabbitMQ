using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;

namespace NewTask
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "task_queue2",
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null
                    );

                    // var message = GetMessage(args);
                    Random rnd = new Random();
                    var counter = 1;

                    while (true)
                    {

                        var message = "Message" + counter;
                        int looper = rnd.Next(1, 20);

                        for (int i = 0; i < looper; i++)
                        {
                            message += ".";
                        }

                        var body = Encoding.UTF8.GetBytes(message);

                        var properties = channel.CreateBasicProperties();
                        properties.Persistent = true;

                        channel.BasicPublish(exchange: "", routingKey: "task_queue", basicProperties: properties, body: body);
                        Console.WriteLine("[x] Sent {0}", message);
                        Thread.Sleep(2000);
                        counter += 1;
                    }
                }
            }
            Console.WriteLine("Press [enter] to exit");
            Console.ReadLine();
        }

        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "Hello World");
        }
    }
}
