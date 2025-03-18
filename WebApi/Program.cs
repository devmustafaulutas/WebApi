using NLog;
using NLog.Web;
using Services.Contracts;
using WebApi.Extentions;


var builder = WebApplication.CreateBuilder(args);
// Add Logger

var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
logger.Debug("Application starting...");
// Default log sağlayıcılarını kapatıp nlog'u entegre etme 
builder.Logging.ClearProviders();
builder.Host.UseNLog();

// Add services to the container.
builder.Services.AddControllers()
    .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly)
                .AddNewtonsoftJson();
builder.Services.AddControllers();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureLoggerService();
builder.Services.AddAutoMapper(typeof(Program));

// Veritabanı
builder.Services.ConfigureMySqlContext(builder.Configuration);


var app = builder.Build();
//app oluştuktan sonra ihtiyaç duyduğumuz servisler böyle eklenicek.

// ----------
var loggerService = app.Services.GetRequiredService<ILoggerService>();
app.ConfigureExceptionHandler(loggerService);
// ----------


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if(app.Environment.IsProduction())
{
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
