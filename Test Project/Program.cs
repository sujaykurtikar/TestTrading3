using Serilog;

var builder = WebApplication.CreateBuilder(args);

var _logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext().CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(_logger);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // Ensure console logging is added for debugging


// Add services to the container.
builder.Services.AddControllers();

// Register the services for dependency injection
//builder.Services.AddSingleton(new LoggerFactory ("logs/log.txt")); // Adjust the path if needed
builder.Services.AddSingleton<InMemoryLogService>();
builder.Services.AddSingleton<Test_Project.Controllers.ControlController.ITaskStateService, Test_Project.Controllers.ControlController.TaskStateService>(); 
builder.Services.AddSingleton<PeriodicTaskService>(); // Register PeriodicTaskService
builder.Services.AddSingleton<HistoricalDataFetcher>();
// Register the background service
builder.Services.AddHostedService<PeriodicTaskService>();

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
app.Logger.LogInformation("Starting the app");
app.Run();
