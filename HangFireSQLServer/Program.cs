using Hangfire;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .Build();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = configuration["ConnectionStrings:DefaultConnection"];
builder.Services.AddHangfire(configuration => configuration
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(connectionString));

// Define a quantidade de retentativas aplicadas a um job com falha.
// Por padrão serão 10, aqui estamos abaixando para duas com intervalo de 5 minutos.
GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute
{
    Attempts = 3,
    DelaysInSeconds = new int[] { 300 }
});

builder.Services.AddHangfireServer();

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

// Para incluir o dashboard
app.UseHangfireDashboard();

app.Run();
