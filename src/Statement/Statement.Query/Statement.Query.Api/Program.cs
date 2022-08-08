
using Confluent.Kafka;
using Core.Consumers;
using Core.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Statement.Query.Api.Queries;
using Statement.Query.Domain.Entities;
using Statement.Query.Domain.Repositories;
using Statement.Query.Infrastructure.Consumers;
using Statement.Query.Infrastructure.DataAccess;
using Statement.Query.Infrastructure.Dispatchers;
using Statement.Query.Infrastructure.Handlers;
using Statement.Query.Infrastructure.Repositories;
using EventHandler = Statement.Query.Infrastructure.Handlers.EventHandler;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Action<DbContextOptionsBuilder> configureDbContext = o => o.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
builder.Services.AddDbContext<DatabaseContext>(configureDbContext);
builder.Services.AddSingleton<DatabaseContextFactory>(new DatabaseContextFactory(configureDbContext));

// create database and tables
var dataContext = builder.Services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
dataContext.Database.EnsureCreated();

builder.Services.AddScoped<IStatementRepository, StatementRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IQueryHandler, QueryHandler>();
builder.Services.AddScoped<IEventHandler, EventHandler>();
builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection(nameof(ConsumerConfig)));
builder.Services.AddScoped<IEventConsumer, EventConsumer>();

// register query handler methods
var queryHandler = builder.Services.BuildServiceProvider().GetRequiredService<IQueryHandler>();
var dispatcher = new QueryDispatcher();
dispatcher.RegisterHandler<FindAllStatementsQuery>(queryHandler.HandleAsync);
dispatcher.RegisterHandler<FindStatementByIdQuery>(queryHandler.HandleAsync);
dispatcher.RegisterHandler<FindStatementsByAuthorQuery>(queryHandler.HandleAsync);
dispatcher.RegisterHandler<FindStatementsWithCommentsQuery>(queryHandler.HandleAsync);
dispatcher.RegisterHandler<FindStatementsWithLikesQuery>(queryHandler.HandleAsync);
builder.Services.AddScoped<IQueryDispatcher<StatementEntity>>(_ => dispatcher);

builder.Services.AddControllers();
builder.Services.AddHostedService<ConsumerHostedService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
