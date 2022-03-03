using Digipolis.Serilog.Elk.Configuration;
using District09.Messaging.AMQP.Extensions;
using District09.Servicefactory.Test.Api.Handlers;
using District09.Servicefactory.Test.Api.Processors;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Host.UseDigipolisSerilog();

builder.Services.AddMessaging(builder.Configuration,
    opts => opts
        .WithListener<MyData, MyMessageHandler>("some.queue.mydata")
        .WithPreProcessor<MyPreProcessor>()
        .WithPostProcessor<MyPostProcessor>()
        .WithPublisher<MyData>("some.queue.mydata")
        .Build());

var app = builder.Build();

app.MapControllers();

app.Run();