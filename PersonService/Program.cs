using PersonService.Data;
using Microsoft.EntityFrameworkCore;
using PersonService.Services;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<IContactInfoService, PersonService.Services.ContactInfoService>();


// Swagger ve Controller desteði
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Servisler
builder.Services.AddScoped<IPersonService, PersonService.Services.PersonService>();
builder.Services.AddDbContext<PhoneBookDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // https://localhost:44393/swagger
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers(); 



app.Run();


