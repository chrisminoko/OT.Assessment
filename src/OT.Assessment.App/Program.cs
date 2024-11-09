using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OT.Assessment.Repository.Implementation;
using OT.Assessment.Repository.Interface;
using OT.Assessment.Services.BusinessLogic.Implementation;
using OT.Assessment.Services.BusinessLogic.Interfaces;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckl
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddScoped<IUnitOfWork>(sp =>
{
    var connectionString = configuration.GetConnectionString("DatabaseConnection");
    var connection = new SqlConnection(connectionString);
    return new UnitOfWork(connection);
});

builder.Services.AddScoped<IPlayerService, PlayerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opts =>
    {
        opts.EnableTryItOutByDefault();
        opts.DocumentTitle = "OT Assessment App";
        opts.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
