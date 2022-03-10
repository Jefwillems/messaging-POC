using System.Text.Json.Serialization;

namespace District09.Servicefactory.Test.Api.Handlers;

public class MySecondData
{
    [JsonPropertyName("test")]
    public string Test { get; set; }
}