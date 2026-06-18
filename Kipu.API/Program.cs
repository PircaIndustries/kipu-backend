using Kipu.API.Document.Application.Internal.CommandServices;
using Kipu.API.Document.Application.Internal.QueryServices;
using Kipu.API.Document.Application.Services;
using Kipu.API.Document.Domain.Repositories;
using Kipu.API.Document.Infraestructure.Persistence.EFC.Repositories;
using Kipu.API.IAM.Application.Internal.CommandServices;
using Kipu.API.IAM.Application.Internal.QueryServices;
using Kipu.API.IAM.Application.Services;
using Kipu.API.IAM.Domain.Repositories;
using Kipu.API.IAM.Infrastructure.Persistence.EFC.Repositories;
using Kipu.API.IAM.Infrastructure.Services;
using Kipu.API.Logistics.Application.Internal.CommandServices;
using Kipu.API.Logistics.Application.Internal.QueryServices;
using Kipu.API.Logistics.Application.Services;
using Kipu.API.Logistics.Domain.Repositories;
using Kipu.API.Logistics.Infrastructure.Persistence.EFC.Repositories;
using Kipu.API.Projects.Application.Internal.CommandServices;
using Kipu.API.Projects.Application.Internal.QueryServices;
using Kipu.API.Projects.Application.Services;
using Kipu.API.Projects.Domain.Repositories;
using Kipu.API.Projects.Infrastructure.Persistence.EFC.Repositories;
using Kipu.API.Resources;
using Kipu.API.Shared.Domain.Repositories;
using Kipu.API.Shared.Infrastructure.Interfaces.ASP.Configuration;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Kipu.API.Team.TeamUser.application.Internal.CommandServices;
using Kipu.API.Team.TeamUser.application.Internal.QueryServices;
using Kipu.API.Team.TeamUser.application.Services;
using Kipu.API.Team.TeamUser.domain.Repositories;
using Kipu.API.Team.TeamUser.Infraestructure.Persistence.EFC.Repositories;
using Kipu.API.Team.TeamWorker.Application.Internal.CommandServices;
using Kipu.API.Team.TeamWorker.Application.Internal.QueryServices;
using Kipu.API.Team.TeamWorker.Application.Services;
using Kipu.API.Team.TeamWorker.Domain.Repositories;
using Kipu.API.Team.TeamWorker.Infraestructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Add services to the container.

builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()))
    .AddDataAnnotationsLocalization();

// Register RFC 7807 ProblemDetails payloads for centralized exception handling.
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        if (context.ProblemDetails.Status is null or >= 500)
        {
            var localizer = context.HttpContext.RequestServices.GetRequiredService<IStringLocalizer<SharedResource>>();
            context.ProblemDetails.Title ??= localizer["UnexpectedServerError"].Value;
            context.ProblemDetails.Detail ??= localizer["UnexpectedErrorProcessingRequest"].Value;
        }
    };
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(
    c =>
    {
        c.EnableAnnotations();
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

// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

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

// Dependency Injections

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

// Logistics Bounded Context Dependency Injections

builder.Services.AddScoped<IMaterialInventoryRepository, MaterialInventoryRepository>();
builder.Services.AddScoped<IMaterialInventoryCommandService, MaterialInventoryCommandService>();
builder.Services.AddScoped<IMaterialInventoryQueryService, MaterialInventoryQueryService>();

builder.Services.AddScoped<IMaterialCatalogRepository, MaterialCatalogRepository>();
builder.Services.AddScoped<IMaterialCatalogCommandService, MaterialCatalogCommandService>();
builder.Services.AddScoped<IMaterialCatalogQueryService, MaterialCatalogQueryService>();

builder.Services.AddScoped<IMaterialCategoryRepository, MaterialCategoryRepository>();
builder.Services.AddScoped<IMaterialCategoryCommandService, MaterialCategoryCommandService>();
builder.Services.AddScoped<IMaterialCategoryQueryService, MaterialCategoryQueryService>();

builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<ISupplierCommandService, SupplierCommandService>();
builder.Services.AddScoped<ISupplierQueryService, SupplierQueryService>();

builder.Services.AddScoped<IMaterialRequestRepository, MaterialRequestRepository>();
builder.Services.AddScoped<IMaterialRequestCommandService, MaterialRequestCommandService>();
builder.Services.AddScoped<IMaterialRequestQueryService, MaterialRequestQueryService>();

builder.Services.AddScoped<ISupplierOfferRepository, SupplierOfferRepository>();
builder.Services.AddScoped<ISupplierOfferCommandService, SupplierOfferCommandService>();
builder.Services.AddScoped<ISupplierOfferQueryService, SupplierOfferQueryService>();

// Team UsersBounded Context Dependency Injections 

builder.Services.AddScoped<ITeamUserRepository, TeamUserRepository>();
builder.Services.AddScoped<ITeamUserCommandService, TeamUserCommandService>();
builder.Services.AddScoped<ITeamUserQueryService, TeamUserQueryService>();

// Team Workers Bounded Context Dependency Injections 

builder.Services.AddScoped<ITeamWorkerRepository, TeamWorkerRepository>();
builder.Services.AddScoped<ITeamWorkerCommandService, TeamWorkerCommandService>();
builder.Services.AddScoped<ITeamWorkerQueryService, TeamWorkerQueryService>();

// Documents Bounded Context Dependency Injections 

builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddScoped<IDocumentCommandService, DocumentCommandService>();
builder.Services.AddScoped<IDocumentQueryService, DocumentQueryService>();

// Budget Bounded Context Dependencies Injections
builder.Services.AddScoped<Kipu.API.Budget.Domain.Repositories.IBudgetItemRepository, Kipu.API.Budget.Infraestructure.BudgetItemRepository>();
builder.Services.AddScoped<Kipu.API.Budget.Application.Services.IBudgetCommandService, Kipu.API.Budget.Application.Internal.CommandServices.BudgetCommandService>();
builder.Services.AddScoped<Kipu.API.Budget.Application.Services.IBudgetQueryService, Kipu.API.Budget.Application.Internal.QueryServices.BudgetQueryService>();

// Progress Bounded Context Dependencies Injections
builder.Services.AddScoped<Kipu.API.Progress.Domain.Repositories.IProgressItemRepository, Kipu.API.Progress.Infrastructure.ProgressItemRepository>();
builder.Services.AddScoped<Kipu.API.Progress.Application.Services.IProgressCommandService, Kipu.API.Progress.Application.Internal.CommandServices.ProgressCommandService>();
builder.Services.AddScoped<Kipu.API.Progress.Application.Services.IProgressQueryService, Kipu.API.Progress.Application.Internal.QueryServices.ProgressQueryService>();

var app = builder.Build();

// Verify Database Objects are created
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAllPolicy");

// Localization Configuration
string[] supportedCultures = ["en", "es"];
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
localizationOptions.ApplyCurrentCultureToResponseHeaders = true;
app.UseRequestLocalization(localizationOptions);

app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

app.Run();