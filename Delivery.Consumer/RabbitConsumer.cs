using Delivery.Application.Interfaces.Customers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Delivery.Consumer;
public static class RabbitConsumer
{
    public record ConsumerModel
    {
        public string Email { get; set; }
    }
    public static void Consume(ISendEmailUseCase sendEmailUseCase)
    {
        ConnectionFactory factory = new()
        {
            UserName = "guest",
            Password = "guest",
            VirtualHost = "/",
            HostName = "localhost",
            Port = 5672
        };

        IConnection conn = factory.CreateConnection();

        IModel channel = conn.CreateModel();
        channel.ExchangeDeclare(exchange: "exchange_default_1", type: ExchangeType.Direct, durable: false, autoDelete: false, null);
        channel.QueueDeclare(queue: "queue_pedido_pronto", durable: true, exclusive: false, autoDelete: false, null);
        channel.QueueBind(queue: "queue_pedido_pronto", exchange: "exchange_default_1", routingKey: "key_default", null);

        var consumer = new EventingBasicConsumer(channel);        
        consumer.Received += (ch, ea) =>
        {
            var body = ea.Body.ToArray();
            var messageSerialize = Encoding.UTF8.GetString(body);
            var message = JsonSerializer.Deserialize<ConsumerModel>(messageSerialize);
            if(!string.IsNullOrEmpty(message.Email))
                sendEmailUseCase.Execute(new Domain.DTOs.SendEmailDTO { Email = message.Email, Content = "Seu pedido está pronto.", Subject = "Pedido pronto" });
            Console.WriteLine(" Recebendo a seguinte mensagem: {0}", message);
            channel.BasicAck(ea.DeliveryTag, false);
            Thread.Sleep(1000);

        };
        string consumerTag = channel.BasicConsume(queue: "queue_pedido_pronto", autoAck: false, consumer: consumer);
        Console.WriteLine("Iniciando consumer");
    }
}


