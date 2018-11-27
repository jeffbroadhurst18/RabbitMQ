﻿using System;
using RabbitMQ.Client;
using System.Text;

namespace Send
{
    // class Program
    // {
    //     static void Main(string[] args)
    //     {
    //         Console.WriteLine("Hello World!");
    //     }
    // }

    class Send {
        public static void Main() {
            var factory = new ConnectionFactory() { HostName="localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel()){
                    channel.QueueDeclare(queue: "hello",
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null
                    );

                    string message = "Hello Cheeky";
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange:"",routingKey:"hello",basicProperties:null,body:body);
                    Console.WriteLine("[x] Sent {0}",message);
                }
            }
            Console.WriteLine("Press [enter] to exit");
            Console.ReadLine();
        }
    }
}
