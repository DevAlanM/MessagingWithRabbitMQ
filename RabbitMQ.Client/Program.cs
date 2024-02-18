using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

const string EXCHANGE = "my-rabbitmq";

var person = new Person("Alan Martins", "123.456.789-10", new DateTime(2001, 10, 30));

var connectionFactory = new ConnectionFactory
{
    HostName = "localhost",
};

var connection = connectionFactory.CreateConnection("my-rabbitmq");

var channel = connection.CreateModel();

var json = JsonSerializer.Serialize(person);
var byteArray = Encoding.UTF8.GetBytes(json);

channel.BasicPublish(EXCHANGE, routingKey: "hr.person-created", basicProperties: null, byteArray);

Console.WriteLine($"Message published: {json}");

var consumerChannel = connection.CreateModel();

var consumer = new EventingBasicConsumer(consumerChannel);

consumer.Received += (sender, eventArgs) =>
{
    var contentArray = eventArgs.Body.ToArray();
    var contentString = Encoding.UTF8.GetString(contentArray);

    var message = JsonSerializer.Deserialize<Person>(contentString);

    Console.WriteLine($"Message received: {contentString}");

    consumerChannel.BasicAck(eventArgs.DeliveryTag, false);
};

consumerChannel.BasicConsume(queue: "person-created", autoAck: false, consumer);

Console.ReadLine();

class Person
{
    public Person(string fullName, string document, DateTime birthDate)
    {
        FullName = fullName;
        Document = document;
        BirthDate = birthDate;
    }

    public string FullName { get; set; }
    public string Document { get; set; }
    public DateTime BirthDate { get; set; }
}
