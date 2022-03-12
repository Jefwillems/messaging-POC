using CorrelationId;
using CorrelationId.DependencyInjection;
using Digipolis.Serilog.Elk.Configuration;
using District09.Messaging.Extensions;
using District09.Messaging.CorrelationId.Extensions;
using District09.Servicefactory.Test.Api.Handlers;
using District09.Servicefactory.Test.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Host.UseDigipolisSerilog();

builder.Services.AddDefaultCorrelationId(opts => { opts.RequestHeader = "CorrelationId"; });

builder.Services.AddMessaging(builder.Configuration,
    opts =>
    {
        opts
            .WithCorrelationId<MyData>()
            .WithListenerMiddleware<MyDataMiddleware, MyData>() // For MyData processing -> first execute our middleware (for putting data in context or something)
            .WithListener<MyData, MyMessageHandler>("some.queue.mydata") // only after middleware is executed, handle the message
            .WithPublisher<MyData>("some.queue.mydata");

        opts.WithListener<MySecondData, MySecondHandler>("some.queue.seconddata")
            .WithListenerMiddleware<MySecondMiddleware, MySecondData>() // in this case, middleware is executed after the message handler
            .WithPublisher<MySecondData>("some.queue.seconddata");

        return opts.Build();
    });

var app = builder.Build();

app.UseCorrelationId();
app.MapControllers();

app.Run();