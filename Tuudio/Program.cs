using Tuudio.Extensions;
using Tuudio.Infrastructure.Data;
using Tuudio.Infrastructure.Services.Interfaces;
using Tuudio.Infrastructure.Services.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("TuudioDbConnection");

builder.RegisterServices();
builder.Services.ConfigureDbContext(connectionString);

var app = builder.Build();

app.RegisterMiddleware();
app.UseAuthorization();
//app.MapControllers();

app.Run();
