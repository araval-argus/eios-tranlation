using eios_tranlation.businesslogic.Features.Label;
using eios_tranlation.businesslogic.Features.Language;
using eios_tranlation.businesslogic.Helpers;
using eios_tranlation.core.Constants;
using eios_tranlation.core.ResponseMiddleware;
using eios_tranlation.infrastructure.ServiceImplementation;
using eios_translation.businesslogic.ServiceInterfaces;
using eios_translation.infrastructure;
using eios_translation.infrastructure.DbContext;
using eios_translation.infrastructure.ServiceImplementation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
CommonSettings.AppSettings = builder.Configuration.GetSection("AppSettings").Get<ApplicationSettings>();
CommonSettings.AzureTranslationSettings = builder.Configuration.GetSection("AzureTranslationSettings").Get<AzureTranslationSettings>();

builder.Services.AddDbContext<EIOSTranslationContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")), ServiceLifetime.Scoped);
builder.Services.AddMediatR(typeof(GetAllLanguagesCommand));



// Add MediatR Authorization Pipeline Handlers
builder.Services.RegisterAuthorizationHandlers(new[] { typeof(GetAllLanguagesCommand).Assembly }, ServiceLifetime.Scoped);

// Register IHttpContextAccessor for services to get access to the HttpContext.
builder.Services.AddHttpContextAccessor();

// Register All Services
builder.Services.RegisterAllTypesWithBaseInterface<IBaseService>(new[] { typeof(LanguageService).Assembly }, ServiceLifetime.Scoped);

// Add services to the container.

builder.Services.AddControllersWithViews().AddFluentValidation(cfg =>
{
    cfg.RegisterValidatorsFromAssemblyContaining(typeof(InsertLanguageCommand));
    cfg.ImplicitlyValidateChildProperties = true;
    cfg.DisableDataAnnotationsValidation = true;
});

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    },
                });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Please insert Subscription Reference with Bearer into field",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
    });

    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EIOS Translation API", Version = "v1" });
});

// Add MediatR Pipelines.
builder.Services.AddMediatRPipelines();

// Add MediatR Authorization Pipeline Handlers
builder.Services.RegisterAuthorizationHandlers(new[] { typeof(InsertLanguageCommand).Assembly }, ServiceLifetime.Scoped);

// Register IHttpContextAccessor for services to get access to the HttpContext.
builder.Services.AddHttpContextAccessor();

// Add AutoMapper Profile
builder.Services.AddAutoMapper(typeof(AutoMappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseApiResponseWrapperMiddleware();
app.UseCors(policy => policy.AllowAnyHeader()
                            .AllowAnyMethod()
                            .SetIsOriginAllowed(origin => true)
                            .AllowCredentials());

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

//app.MapFallbackToFile("index.html");

app.Run();
