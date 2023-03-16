using Shop.API;
using Shop.BLL.AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.ConfigureServices();

builder.Services.AddControllers();

builder.Services.AddAuthentication();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);

builder.Services.ConfigureSeedDataAsync().ConfigureAwait(false).GetAwaiter().GetResult();

builder.Services.AddEndpointsApiExplorer();

builder.Services.ConfigureJsonOptions();
builder.Services.ConfigureSwagger();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI( c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shop API for Maxsoft");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
