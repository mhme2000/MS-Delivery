using Delivery.Application.Interfaces.Customers;
using Delivery.Application.UseCases.Customers;
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
            UserName = "twlvsbxr",
            Password = "vbeQt4hymocx2OG3lCzR4t6qI0OTRjKA",
            VirtualHost = "twlvsbxr",
            HostName = "fish-01.rmq.cloudamqp.com",
            Port = 5672
        };

        IConnection conn = factory.CreateConnection();

        ListenQueuePedidoPronto(conn, sendEmailUseCase);
        ListenQueuePedidoPago(conn, sendEmailUseCase);
        ListenQueuePedidoCancelado(conn, sendEmailUseCase);

        Console.WriteLine("Iniciando consumer");
    }

    private static void ListenQueuePedidoPronto(IConnection conn, ISendEmailUseCase sendEmailUseCase)
    {
        IModel channel = conn.CreateModel();
        channel.ExchangeDeclare(exchange: "exchange_pedido_pronto", type: ExchangeType.Direct, durable: false, autoDelete: false, null);
        channel.QueueDeclare(queue: "queue_pedido_pronto_1", durable: true, exclusive: false, autoDelete: false, null);
        channel.QueueBind(queue: "queue_pedido_pronto_1", exchange: "exchange_pedido_pronto", routingKey: "key_default", null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (ch, ea) =>
        {
            var body = ea.Body.ToArray();
            var messageSerialize = Encoding.UTF8.GetString(body);
            var message = JsonSerializer.Deserialize<ConsumerModel>(messageSerialize, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (message != null && !string.IsNullOrEmpty(message.Email))
                sendEmailUseCase.Execute(new Domain.DTOs.SendEmailDTO { Email = message.Email, Content = "Seu pedido está pronto.", Subject = "Pedido pronto" });
            Console.WriteLine(" Recebendo a seguinte mensagem: {0}", message);
            channel.BasicAck(ea.DeliveryTag, false);
            Thread.Sleep(1000);

        };
        string consumerTag = channel.BasicConsume(queue: "queue_pedido_pronto_1", autoAck: false, consumer: consumer);
    }

    private static void ListenQueuePedidoPago(IConnection conn, ISendEmailUseCase sendEmailUseCase)
    {
        IModel channel = conn.CreateModel();
        channel.ExchangeDeclare(exchange: "exchange_pedido_pago", type: ExchangeType.Fanout, durable: false, autoDelete: false, null);
        channel.QueueDeclare(queue: "queue_pedido_pago_1", durable: true, exclusive: false, autoDelete: false, null);
        channel.QueueBind(queue: "queue_pedido_pago_1", exchange: "exchange_pedido_pago", routingKey: "key_default", null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (ch, ea) =>
        {
            var body = ea.Body.ToArray();
            var messageSerialize = Encoding.UTF8.GetString(body);
            var message = JsonSerializer.Deserialize<ConsumerModel>(messageSerialize, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (message != null && !string.IsNullOrEmpty(message.Email))
                sendEmailUseCase.Execute(new Domain.DTOs.SendEmailDTO { Email = message.Email, Content = "Seu pagamento foi confirmado, seu pedido entrou na fila de produção.", Subject = "Pedido pago" });
            Console.WriteLine(" Recebendo a seguinte mensagem: {0}", message);
            channel.BasicAck(ea.DeliveryTag, false);
            Thread.Sleep(1000);

        };
        string consumerTag = channel.BasicConsume(queue: "queue_pedido_pago_1", autoAck: false, consumer: consumer);
    }

    private static void ListenQueuePedidoCancelado(IConnection conn, ISendEmailUseCase sendEmailUseCase)
    {
        IModel channel = conn.CreateModel();
        channel.ExchangeDeclare(exchange: "exchange_pedido_cancelado", type: ExchangeType.Fanout, durable: false, autoDelete: false, null);
        channel.QueueDeclare(queue: "queue_pedido_cancelado_1", durable: true, exclusive: false, autoDelete: false, null);
        channel.QueueBind(queue: "queue_pedido_cancelado_1", exchange: "exchange_pedido_cancelado", routingKey: "key_default", null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (ch, ea) =>
        {
            var body = ea.Body.ToArray();
            var messageSerialize = Encoding.UTF8.GetString(body);
            var message = JsonSerializer.Deserialize<ConsumerModel>(messageSerialize, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (message != null && !string.IsNullOrEmpty(message.Email))
                sendEmailUseCase.Execute(new Domain.DTOs.SendEmailDTO { Email = message.Email, Content = "Seu pagamento foi recusado e seu pedido foi cancelado.", Subject = "Pedido cancelado" });
            Console.WriteLine(" Recebendo a seguinte mensagem: {0}", message);
            channel.BasicAck(ea.DeliveryTag, false);
            Thread.Sleep(1000);

        };
        string consumerTag = channel.BasicConsume(queue: "queue_pedido_cancelado_1", autoAck: false, consumer: consumer);
    }
}


