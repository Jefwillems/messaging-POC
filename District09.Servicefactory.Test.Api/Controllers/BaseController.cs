using District09.Messaging.AMQP.Contracts;
using District09.Servicefactory.Test.Api.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace District09.Servicefactory.Test.Api.Controllers;

[ApiController]
[Route("api")]
public class BaseController : ControllerBase
{
    private readonly IMessagePublisher<MyData> _publisher;

    public BaseController(IMessagePublisher<MyData> publisher)
    {
        _publisher = publisher;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var a = new MyData() { Hello = "World" };
        _publisher.PublishMessage(a);
        return Ok(a);
    }
}