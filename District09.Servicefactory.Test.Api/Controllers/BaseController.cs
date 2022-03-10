using District09.Messaging.AMQP.Contracts;
using District09.Servicefactory.Test.Api.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace District09.Servicefactory.Test.Api.Controllers;

[ApiController]
[Route("api")]
public class BaseController : ControllerBase
{
    private readonly IMessagePublisher<MyData> _publisher;
    private readonly IMessagePublisher<MySecondData> _secondPublisher;

    public BaseController(IMessagePublisher<MyData> publisher,
        IMessagePublisher<MySecondData> secondPublisher)
    {
        _publisher = publisher;
        _secondPublisher = secondPublisher;
    }

    [HttpGet]
    public IActionResult Index(int i = 1)
    {
        if (i != 1)
        {
            var a = new MySecondData() { Test = "test123" };
            _secondPublisher.PublishMessage(a);
            return Ok(a);
        }
        else
        {
            var a = new MyData() { Hello = "World" };
            _publisher.PublishMessage(a);
            return Ok(a);
        }
    }
}