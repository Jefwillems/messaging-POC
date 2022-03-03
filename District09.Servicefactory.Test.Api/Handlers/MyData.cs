using System.Text.Json.Serialization;

namespace District09.Servicefactory.Test.Api.Handlers;

public class MyData
{
    [JsonPropertyName("hello")]
    public string Hello { get; set; }
}