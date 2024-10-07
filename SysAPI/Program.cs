using MessageClient.Factory;
using MessageClient;
using Helpers.Messages;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var easyNetQFactory = new EasyNetQFactory();


builder.Services.AddSingleton<MessageClient<ValutaResponseMessage>>(easyNetQFactory.CreateSendReceiveMessageClient<ValutaResponseMessage>("SysAPI").Connect());
builder.Services.AddSingleton<MessageClient<ValutaResponseMessage>>(easyNetQFactory.CreateTopicMessageClient<ValutaResponseMessage>("SysAPI", "").Connect());
builder.Services.AddSingleton<MessageClient<ValutaRequestMessage>>(easyNetQFactory.CreatePubSubMessageClient<ValutaRequestMessage>("SysAPI").Connect());

builder.Services.AddControllers();
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
