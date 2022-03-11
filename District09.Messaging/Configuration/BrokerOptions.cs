namespace District09.Messaging.Configuration;

public class BrokerOptions
{
    public const string Prefix = "AmqConfiguration:Broker";
    public string Uri { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}