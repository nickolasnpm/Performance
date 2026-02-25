using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Performance.Application.Configuration;
using Performance.Application.Interface.Repository;
using Performance.Application.Interface.Services;
using Performance.Application.Interface.UnitOfWork;
using Performance.Domain.Entity;
using Performance.Domain.Services;
using Performance.Infrastructure;
using Performance.Infrastructure.Repositories;
using Performance.Infrastructure.UnitOfWork;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region OData configuration
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
        .AddRouteComponents("odata", edmModel));
#endregion

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

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
        }
    )
    .EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection("CacheSettings"));

builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepositories, UserRepositories>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
