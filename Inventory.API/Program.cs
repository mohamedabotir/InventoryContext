using System.Reflection;
using Application.Handlers;
using Application.Models;
using Application.UseCases;
using Common.Events;
using Common.Repository;
using Common.Result;
using Confluent.Kafka;
using Domain.Repository;
using GraphQL;
using GraphQL.Server.Ui.Playground;
using GraphQL.Types;
using Infrastructure.Consumer;
using Infrastructure.Context;
using Infrastructure.MessageBroker;
using Infrastructure.MessageBroker.Producers;
using Infrastructure.Mongo;
using Infrastructure.Repository;
using Inventory.API.GraphQL.Query;
using Inventory.API.GraphQL.Schemas;
using Inventory.API.GraphQL.Types;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using EventHandler = Infrastructure.Consumer.EventHandler;

var builder = WebApplication.CreateBuilder(args);
Action<DbContextOptionsBuilder> dbContextConfiguration = (e => e.UseSqlServer(builder.Configuration.GetConnectionString("ItemContext")));
builder.Services.AddDbContext<ItemContext>(dbContextConfiguration);
builder.Services.AddSingleton(new ItemContextFactory(dbContextConfiguration));

builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection("ConsumerConfig"));
builder.Services.Configure<InventoryTopic>(builder.Configuration.GetSection("Topic"));
builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection("ProducerConfig"));
builder.Services.Configure<InventoryMongoConfig>(builder.Configuration.GetSection("MongoConfig"));


BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
BsonClassMap.RegisterClassMap<OrderShipped>();



builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddTransient<IItemRepository , ItemRepository>();
builder.Services.AddTransient<IEventRepository , EventRepository>();
builder.Services.AddTransient<IItemUseCase,ItemUseCase>();
builder.Services.AddTransient<IUnitOfWork,UnitOfWork>();
// GraphQL
builder.Services.AddTransient<ItemQuery>();
builder.Services.AddTransient<ItemType>();
builder.Services.AddTransient<StockType>();
builder.Services.AddTransient<ISchema,ItemSchema>();
builder.Services.AddGraphQL(b => b
    .AddAutoSchema<ItemQuery>()  
    .AddSystemTextJson()
    .AddDataLoader());
builder.Services.AddScoped<IEventHandler,EventHandler>();
builder.Services.AddScoped<IEventConsumer<EventConsumer>, EventConsumer>();
builder.Services.AddScoped<IRequestHandler<CreateItemCommand, Result>, CreateItemHandler>();
builder.Services.AddScoped<IEventStore,InventoryEventStore>();
builder.Services.AddScoped<IProducer,Producer>();

builder.Services.AddHostedService<ConsumerHostingService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseGraphQL<ISchema>("/graphql");
app.UseGraphQLPlayground("/graphql-ui" , new PlaygroundOptions()
{
    GraphQLEndPoint = "/graphql"
});
app.MapPost("/item", async (CreateItemCommand item,IMediator mediator) => 
    {
        var createdItem = await mediator.Send(item);
        return createdItem.IsFailure ? Results.BadRequest(createdItem.Message) : Results.Created();
    })
    .WithName("item creation with stock")
    .WithOpenApi();

app.Run();
