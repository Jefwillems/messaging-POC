using System.Text.Json.Serialization;

namespace District09.Servicefactory.Test.Api;

public class MyData
{
    [JsonPropertyName("test")]
    public string Test { get; set; }
}