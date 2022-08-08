
using Confluent.Kafka;
using Core.Domain;
using Core.Events;
using Core.Handlers;
using Core.Infrastructure;
using Core.Producers;
using MongoDB.Bson.Serialization;
using Statement.Command.Api.Commands;
using Statement.Command.Domain.Aggregates;
using Statement.Command.Infrastructure.Config;
using Statement.Command.Infrastructure.Dispatchers;
using Statement.Command.Infrastructure.Handlers;
using Statement.Command.Infrastructure.Producers;
using Statement.Command.Infrastructure.Repositories;
using Statement.Command.Infrastructure.Stores;
using Statement.Common.Events;

var builder = WebApplication.CreateBuilder(args);

BsonClassMap.RegisterClassMap<BaseEvent>();
BsonClassMap.RegisterClassMap<StatementCreatedEvent>();
BsonClassMap.RegisterClassMap<StatementUpdatedEvent>();
BsonClassMap.RegisterClassMap<StatementLikedEvent>();
BsonClassMap.RegisterClassMap<CommentAddedEvent>();
BsonClassMap.RegisterClassMap<CommentUpdatedEvent>();
BsonClassMap.RegisterClassMap<CommentRemovedEvent>();
BsonClassMap.RegisterClassMap<StatementRemovedEvent>();

// Add services to the container.
builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));
builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection(nameof(ProducerConfig)));
builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
builder.Services.AddScoped<IEventProducer, EventProducer>();
builder.Services.AddScoped<IEventStore, EventStore>();
builder.Services.AddScoped<IEventSourcingHandler<StatementAggregate>, EventSourcingHandler>();
builder.Services.AddScoped<ICommandHandler, CommandHandler>();

// register command handler methods
var commandHandler = builder.Services.BuildServiceProvider().GetRequiredService<ICommandHandler>();
var dispatcher = new CommandDispatcher();
dispatcher.RegisterHandler<NewStatementCommand>(commandHandler.HandleAsync);
dispatcher.RegisterHandler<EditStatementCommand>(commandHandler.HandleAsync);
dispatcher.RegisterHandler<LikeStatementCommand>(commandHandler.HandleAsync);
dispatcher.RegisterHandler<AddCommentCommand>(commandHandler.HandleAsync);
dispatcher.RegisterHandler<EditCommentCommand>(commandHandler.HandleAsync);
dispatcher.RegisterHandler<RemoveCommentCommand>(commandHandler.HandleAsync);
dispatcher.RegisterHandler<DeleteStatementCommand>(commandHandler.HandleAsync);
dispatcher.RegisterHandler<RestoreReadDbCommand>(commandHandler.HandleAsync);
builder.Services.AddSingleton<ICommandDispatcher>(_ => dispatcher);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
