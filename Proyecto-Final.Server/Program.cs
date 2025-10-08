using Microsoft.EntityFrameworkCore;
using Proyecto_Final.Server;

var builder = WebApplication.CreateBuilder(args);

/* Add CORS support */
builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(
		builder =>
		{
			builder.AllowAnyOrigin() // Not recommended for production
				   .AllowAnyHeader()
				   .AllowAnyMethod();
		});
});

/* Configuracion de Pomelo Entity Framework */

// Replace with your connection string.
var connectionString = "server=localhost;user=root;password=query_sql;database=proyecto_encuesta";

// Replace with your server version and type.
// Use 'MariaDbServerVersion' for MariaDB.
// Alternatively, use 'ServerVersion.AutoDetect(connectionString)'.
// For common usages, see pull request #1233.
var serverVersion = new MySqlServerVersion(ServerVersion.AutoDetect(connectionString));

// Replace 'YourDbContext' with the name of your own DbContext derived class.
builder.Services.AddDbContext<ProyectoEncuestaContext>(
	dbContextOptions => dbContextOptions
		.UseMySql(connectionString, serverVersion)
		// The following three options help with debugging, but should
		// be changed or removed for production.
		.LogTo(Console.WriteLine, LogLevel.Information)
		.EnableSensitiveDataLogging()
		.EnableDetailedErrors()
);

/****/

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(); // Apply the default policy

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
