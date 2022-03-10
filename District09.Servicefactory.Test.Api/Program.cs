using Digipolis.Serilog.Elk.Configuration;
using District09.Messaging.AMQP.Extensions;
using District09.Servicefactory.Test.Api.Handlers;
using District09.Servicefactory.Test.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Host.UseDigipolisSerilog();

builder.Services.AddMessaging(builder.Configuration,
    opts => opts
        .WithMiddleware<MyDataMiddleware, MyData>() // For MyData processing -> first execute our middleware (for putting data in context or something)
        .WithListener<MyData, MyMessageHandler>("some.queue.mydata") // only after middleware is executed, handle the message
        .WithPublisher<MyData>("some.queue.mydata")
        .WithListener<MySecondData, MySecondHandler>("some.queue.seconddata")
        .WithMiddleware<MySecondMiddleware, MySecondData>() // in this case, middleware is executed after the message handler
        .WithPublisher<MySecondData>("some.queue.seconddata")
        .Build());

var app = builder.Build();

app.MapControllers();

app.Run();