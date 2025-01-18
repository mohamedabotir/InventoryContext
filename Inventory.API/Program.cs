using System.Reflection;
using Application.Handlers;
using Application.Models;
using Application.UseCases;
using Common.Repository;
using Common.Result;
using Domain.Repository;
using GraphQL;
using GraphQL.Server.Ui.Playground;
using GraphQL.Types;
using Infrastructure.Context;
using Infrastructure.Repository;
using Inventory.API.GraphQL.Query;
using Inventory.API.GraphQL.Schemas;
using Inventory.API.GraphQL.Types;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ItemContext>(e=>e.UseSqlServer(builder.Configuration
    .GetConnectionString("ItemContext")));
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddTransient<IItemRepository , ItemRepository>();
builder.Services.AddTransient<IItemUseCase,ItemUseCase>();
builder.Services.AddTransient<IUnitOfWork,UnitOfWork>();

builder.Services.AddTransient<ItemQuery>();
builder.Services.AddTransient<ItemType>();
builder.Services.AddTransient<StockType>();
builder.Services.AddTransient<ISchema,ItemSchema>();
builder.Services.AddGraphQL(b => b
    .AddAutoSchema<ItemQuery>()  
    .AddSystemTextJson()
    .AddDataLoader());
builder.Services.AddTransient<IRequestHandler<CreateItemCommand, Result>, CreateItemHandler>();

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
