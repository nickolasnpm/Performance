using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Performance.API.Exceptions;
using Performance.Application.Common.Settings;
using Performance.Application.Interface.Security;
using Performance.Application.Interface.Repository;
using Performance.Application.Interface.Services;
using Performance.Application.Interface.UnitOfWork;
using Performance.Application.Services;
using Performance.Domain.Entity;
using Performance.Infrastructure.Persistence;
using Performance.Infrastructure.Persistence.Extensions;
using Performance.Infrastructure.Persistence.Repositories;
using Performance.Infrastructure.Persistence.UnitOfWork;
using Performance.Infrastructure.Persistence.Interceptors;
using Performance.Infrastructure.Security;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<AuditSaveChangesInterceptor>();

builder.Services.AddDbContextPool<UserDbContext>((sp,options) =>
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
    .AddInterceptors(sp.GetRequiredService<AuditSaveChangesInterceptor>())
);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection("CacheSettings"));
builder.Services.Configure<IdEncryptionSettings>(builder.Configuration.GetSection("IdEncryptionSettings"));

builder.Services.AddScoped<IUserServices, UserServices>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepositories, UserRepositories>();
builder.Services.AddScoped<IIdHelper, IdHelper>();

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
            var errors = context.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .SelectMany(kvp => kvp.Value!.Errors.Select(err => new FieldValidationError
                {
                    Field = kvp.Key,
                    Message = err.ErrorMessage
                }))
                .ToList();

            var problemDetails = new ProblemDetails
            {
                Title = "Request validation failed",
                Status = StatusCodes.Status400BadRequest,
                Detail = $"{errors.Count} validation error(s) occurred.",
                Instance = context.HttpContext.Request.Path,
                Extensions =
                {
                    ["traceId"] = context.HttpContext.TraceIdentifier,
                    ["errors"] = errors
                }
            };

            return new BadRequestObjectResult(problemDetails);
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