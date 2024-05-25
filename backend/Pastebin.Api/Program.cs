using System.Text;
using FluentValidation;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.IdentityModel.Tokens;
using Pastebin.Api.Extensions;
using Pastebin.Api.JsonConverters;
using Pastebin.Api.Middlewares;
using Pastebin.Api.Services;
using Pastebin.Application;
using Pastebin.Common.Options;
using Pastebin.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new DateTimeOffsetJsonConverter());
});

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddTransient<CookieService>();
builder.Services.AddScoped<UserContext>();
builder.Services.AddHttpContextAccessor();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<GoogleOAuthOptions>(builder.Configuration.GetSection("GoogleOAuthOptions"));

builder.Services.AddHttpClient();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.RegisterModules();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = "redis";
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ClockSkew = TimeSpan.Zero,
            ValidateIssuerSigningKey = true,    
            ValidateLifetime = builder.Configuration.GetValue<bool>("JwtSettings:ValidateLifetime"),
            ValidateIssuer = builder.Configuration.GetValue<bool>("JwtSettings:ValidateIssuer"),
            ValidateAudience = builder.Configuration.GetValue<bool>("JwtSettings:ValidateAudience"),
            ValidIssuers = builder.Configuration.GetValue<string[]>("JwtSettings:ValidIssuers"),
            ValidAudiences = builder.Configuration.GetValue<string[]>("JwtSettings:ValidAudiences"),
            IssuerSigningKey = 
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(configure =>
    {
        configure.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapHangfireDashboard();
}

app.UseCustomExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

var apiGroup = app.MapGroup("api");
apiGroup.MapEndpoints();

app.MapGet("test", () => "Auth is working!").RequireAuthorization();
app.Run();