using Voicepoint.Assessment.Services;

var builder = WebApplication.CreateBuilder(args);

// Add system services
var services = builder.Services;
services.AddControllers();

services.AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddCors(options => { options.AddDefaultPolicy(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()); });

// add application specific services
services.AddSingleton<IHelloService, HelloService>();

// create application
var app = builder.Build();
app.UseSwagger()
    .UseSwaggerUI()
    .UseAuthorization()
    .UseCors();

app.MapControllers();
app.Run();