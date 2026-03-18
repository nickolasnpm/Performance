using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Performance.API.Exceptions;
using Performance.Application.Common.Settings;
using Performance.Application.Interface.Repository;
using Performance.Application.Interface.Services;
using Performance.Application.Interface.UnitOfWork;
using Performance.Domain.Entity;
using Performance.Domain.Services;
using Performance.Infrastructure;
using Performance.Infrastructure.Extensions;
using Performance.Infrastructure.Repositories;
using Performance.Infrastructure.UnitOfWork;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<UserDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions
                .EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorNumbersToAdd: null
                )
                .CommandTimeout(300);

            sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "Performance");
        }
    )
    .EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection("CacheSettings"));

builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepositories, UserRepositories>();

var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<User>("UsersOData");
var edmModel = modelBuilder.GetEdmModel();

builder.Services.AddControllers()
    .AddOData(options => options
        .Select()
        .Filter()
        .OrderBy()
        .Count()
        .Expand()
        .SetMaxTop(1000)
        .AddRouteComponents("odata", edmModel))
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            context.HttpContext.Items["ModelState"] = context.ModelState;
            context.HttpContext.Items["Instance"] = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ErrorController>>();
            var errorController = new ErrorController(logger)
            {
                ControllerContext = new ControllerContext { HttpContext = context.HttpContext }
            };
            return errorController.HandleValidationError();
        };
    });

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler(new ExceptionHandlerOptions
{
    ExceptionHandlingPath = "/error",
    SuppressDiagnosticsCallback = context => true,
});
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

if (!app.Environment.IsProduction())
{
    await app.ApplyMigrations();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();