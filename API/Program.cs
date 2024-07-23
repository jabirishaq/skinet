using API.Errors;
using API.Extensions;
using API.Helpers;
using API.Middleware;
using Core.Interfaces;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. //Add the services for the Interfaces
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(); // this will not be used for now, we will use our own documetnation
builder.Services.AddSwaggerDocumentation(); // this will be used, we will use our own documetnation


builder.Services.AddControllers();
//builder.Services.Configure<ApiBehaviorOptions>(options =>
//{
//    options.InvalidModelStateResponseFactory = actionContext => {  
//    var errors = actionContext.ModelState.Where(e=>e.Value.Errors.Count > 0).SelectMany(x => x.Value.Errors).Select(x=>x.ErrorMessage).ToArray();

//        var errorResponse = new ApiValidationErrorResponse
//        {
//            Errors = errors
//        };

//        return new BadRequestObjectResult(errorResponse);

//    };
//});

//builder.Services.AddSwaggerGen(options => {
//    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "API", Version = "v1" });
//    });

builder.Services.AddDbContext<StoreContext> (x => x.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddApplicationServices();
//Interfaces to be added here
//builder.Services.AddScoped<IProductRepository, ProductRepository>();
//builder.Services.AddScoped(typeof(IGenericRepository<>),(typeof(GenericRepository<>)));
builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
    });
});
var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>(); //initiate the added middle ware added customly

app.UseCors("CorsPolicy");

// Configure the HTTP request pipeline. it is the middle ware
//if (app.Environment.IsDevelopment())
//{
    //app.UseSwagger(); 
    //app.UseSwaggerUI();
//}

app.UseSwaggerDocumentation();





app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();

//using the static files that are in wwwroot folder and are bind in the pictureUrl
app.UseStaticFiles();


app.MapControllers();
// Seed the database.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = services.GetRequiredService<StoreContext>();
        context.Database.Migrate(); // Applies any pending migrations
        StoreContextSeed.SeedAsync(context, loggerFactory).Wait();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the database.");
    }
}
app.Run();


// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();

// app.MapGet("/products",()=>{
//     return "rereading all products";

// });

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Products}/{action=GetProducts}/{id?}");

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast =  Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast")
// .WithOpenApi();

// app.Run();

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }
