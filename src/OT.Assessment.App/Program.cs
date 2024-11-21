using System.Reflection;
using Microsoft.EntityFrameworkCore;
using OT.Assessment.App.DataAccess;
using OT.Assessment.App.Interfaces;
using OT.Assessment.App.Interfaces.DatabaseOperations;
using OT.Assessment.App.Interfaces.RabbitMQ;
using OT.Assessment.App.Interfaces.Service;
using OT.Assessment.App.Messaging;
using OT.Assessment.App.OtDbContext;
using OT.Assessment.App.Repository;
using OT.Assessment.App.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckl
builder.Services.AddLogging(loggingBuilder => {
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});
builder.Services.AddScoped<ICasinoWagerService, CasinoWagerService>();
builder.Services.AddSingleton<IPublishWagerAsync, RabbittMqClient> ();
builder.Services.AddScoped<ICasinoWagerDataAccess, CasinoWagerDataAccess>();
builder.Services.AddScoped<IDatabaseOperations,DatabaseOperationsRepository>();
builder.Services.AddDbContext<OtDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection")));
builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

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
