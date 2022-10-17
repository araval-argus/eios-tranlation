using eios_translation.businesslogic.Helpers;
using eios_translation.businesslogic.ServiceInterfaces;
using eios_translation.infrastructure;
using eios_translation.infrastructure.DbContext;
using eios_translation.infrastructure.ServiceImplementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EIOSTranslationContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")), ServiceLifetime.Scoped);

builder.Services.RegisterAllTypesWithBaseInterface<IBaseService>(new[] { typeof(LabelService).Assembly }, ServiceLifetime.Scoped);

// Add services to the container.

builder.Services.AddControllersWithViews();
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
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
