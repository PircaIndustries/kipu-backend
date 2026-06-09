using Kipu.API.IAM.Application.Internal.CommandServices;
using Kipu.API.IAM.Application.Internal.QueryServices;
using Kipu.API.IAM.Application.Services;
using Kipu.API.IAM.Domain.Repositories;
using Kipu.API.IAM.Infrastructure.Persistence.EFC.Repositories;
using Kipu.API.IAM.Infrastructure.Services;
using Kipu.API.Projects.Application.Internal.CommandServices;
using Kipu.API.Projects.Application.Internal.QueryServices;
using Kipu.API.Projects.Application.Services;
using Kipu.API.Projects.Domain.Repositories;
using Kipu.API.Projects.Infrastructure.Persistence.EFC.Repositories;
using Kipu.API.Shared.Domain.Repositories;
using Kipu.API.Shared.Infrastructure.Interfaces.ASP.Configuration;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()));

// Add database Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(
    options =>
    {
        if (connectionString != null)
            if (builder.Environment.IsDevelopment())
                options.UseMySQL(connectionString)
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            else if (builder.Environment.IsProduction())
                options.UseMySQL(connectionString)
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableDetailedErrors();
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1",
            new OpenApiInfo
            {
                Title = "PircaIndustries.Kipu.API",
                Version = "v1",
                Description = "Kipu Platform API",
                TermsOfService = new Uri("https://PircaIndustries.Kipu.com/tos"),
                Contact = new OpenApiContact
                {
                    Name = "PircaIndustries Team",
                    Email = "contact@kipu.com"
                },
                License = new OpenApiLicense
                {
                    Name = "Apache 2.0",
                    Url = new ("https://www.apache.org/licenses/LICENSE-2.0.html")
                }
            });
    });

// Configure Lowercase URLs
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// IAM Bounded Context Dependency Injections
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();

// Projects Bounded Context Dependency Injections
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectItemRepository, ProjectItemRepository>();
builder.Services.AddScoped<IProjectCommandService, ProjectCommandService>();
builder.Services.AddScoped<IProjectQueryService, ProjectQueryService>();

var app = builder.Build();

// Verify Database Objects are created
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAllPolicy");

app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

app.Run();