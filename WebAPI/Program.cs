using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Get the database provider from appsettings.json
var configuration = builder.Configuration;
var databaseProvider = configuration["DatabaseProvider"];

// Add DbContext and configure based on selected database
if (databaseProvider == "SqlServer")
{
    builder.Services.AddDbContext<InvoicikaDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("SqlServerConnection")));
}
else if (databaseProvider == "MySql")
{
    builder.Services.AddDbContext<InvoicikaDbContext>(options =>
        options.UseMySql(configuration.GetConnectionString("MySqlConnection"), new MySqlServerVersion(new Version(8, 0, 26))));
}

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();

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
