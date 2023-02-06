using AgDataCodingAssignment.Application.Contracts;
using AgDataCodingAssignment.Application.ServiceConfiguration;
using AgDataCodingAssignment.Persistence;
using AgDataCodingAssignment.Persistence.Repositories;
using AgDataCodingAssignment.WebFramework.Filters;
using AgDataCodingAssignment.WebFramework.Middlewares;
using AgDataCodingAssignment.WebFramework.Swagger;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

//builder.Services.Configure<DatabaseSettings>(configuration.GetSection(nameof(DatabaseSettings)));


//var databaseSettings = configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();


builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(OkResultAttribute));
    options.Filters.Add(typeof(NotFoundResultAttribute));
    options.Filters.Add(typeof(ContentResultFilterAttribute));
    options.Filters.Add(typeof(ModelStateValidationAttribute));
    options.Filters.Add(typeof(BadRequestResultFilterAttribute));

}).ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; });
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSwagger();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCustomExceptionHandler();
app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
