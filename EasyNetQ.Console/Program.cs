using EasyNetQ;
using MessagingEvents.Shared.Models;
using Newtonsoft.Json;

const string EXCHANGE = "my-rabbitmq";
const string QUEUE = "person-created";
const string ROUTING_KEY = "hr.person-created";

var person = new Person("Alan Martins", "123.456.789-10", new DateTime(2001, 10, 30));

var bus = RabbitHutch.CreateBus("host=localhost");

#region Advanced Publish Messaging

var advanced = bus.Advanced;

var exchange = advanced.ExchangeDeclare(EXCHANGE, type: "topic");
var queue = advanced.QueueDeclare(QUEUE);

advanced.Publish(exchange, ROUTING_KEY, true, new Message<Person>(person));

advanced.Consume<Person>(queue, (msg, info) =>
{
    var json = JsonConvert.SerializeObject(msg.Body);
    Console.WriteLine(json);
});

#endregion

#region Simple Publish Messaging

//await bus.PubSub.PublishAsync(person);

//await bus.PubSub.SubscribeAsync<Person>("hr", msg =>
//{
//    var json = JsonConvert.SerializeObject(msg);
//    Console.WriteLine(json);
//});

#endregion

Console.ReadLine();