using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace RabbitMQClient.Marketing.API.Subscribers
{
    public class CustomerCreatedSubscriber : IHostedService
    {
        private readonly IModel _channel;

        const string EXCHANGE = "my-rabbitmq";
        const string CUSTOMER_CREATED_QUEE = "customer-created";

        public CustomerCreatedSubscriber()
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = "localhost"
            };
            
            var connection = connectionFactory.CreateConnection("my-rabbit-client-consumer");

            _channel = connection.CreateModel();
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (sender, eventArgs) =>
            {
                var contentArray = eventArgs.Body.ToArray();
                var contentString = Encoding.UTF8.GetString(contentArray);

                var @event = JsonSerializer.Deserialize<CustomerCreatedSubscriber>(contentString);

                Console.WriteLine($"Message received:  {contentString}");

                _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume(CUSTOMER_CREATED_QUEE, autoAck: false, consumer);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}