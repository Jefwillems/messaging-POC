using District09.Messaging.AMQP.Extensions;
using District09.Servicefactory.Test.Api;
using District09.Servicefactory.Test.Api.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IDoSomething<DoSomethingA>, DoSomethingA>();
builder.Services.AddTransient<IDoSomething<DoSomethingB>, DoSomethingB>();


builder.Services.AddMessaging(builder.Configuration,
    opts => opts
        .WithListener<MyData, MyMessageHandler>("some.queue.mydata")
        .Build());

var app = builder.Build();

var s = app.Services.GetRequiredService<IDoSomething<DoSomethingA>>();
s.DoSomething();

app.MapGet("/", () => "Hello World!");

app.Run();