using System.Text;
using CargoTrack.Services.Identity.API.Application.Commands;
using CargoTrack.Services.Identity.API.Application.Validators;
using CargoTrack.Services.Identity.API.Infrastructure.Data;
using CargoTrack.Services.Identity.API.Infrastructure.Repositories;
using CargoTrack.Services.Identity.API.Domain.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using CargoTrack.Services.Identity.API.Infrastructure.Middlewares;
using CargoTrack.Services.Identity.API.Application.Behaviors;
using CargoTrack.Services.Identity.API.Domain.Services;
using CargoTrack.Services.Identity.API.Infrastructure.Services;
using CargoTrack.Services.Identity.API.Domain.Events;
using CargoTrack.Services.Identity.API.Application.EventHandlers;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration; // Gereksiz tekrarları önlemek için tanımlandı

// Add services to the container
builder.Services.AddControllers(); // Tüm controller'ları otomatik olarak ekle

// DbContext
builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();

// Domain Services
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

// Event Handlers
builder.Services.AddScoped<IDomainEventHandler<UserCreatedEvent>, UserCreatedEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<UserLockedOutEvent>, UserLockedOutEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<UserRolesUpdatedEvent>, UserRolesUpdatedEventHandler>();

// Swagger Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CargoTrack Identity API",
        Version = "v1",
        Description = "CargoTrack Identity Service API Documentation",
        Contact = new OpenApiContact
        {
            Name = "CargoTrack Team",
            Email = "support@cargotrack.com"
        }
    });

    // Controller'ları gruplandır
    c.TagActionsBy(api =>
    {
        if (api.GroupName != null)
            return new[] { api.GroupName };

        var controllerName = api.ActionDescriptor.RouteValues["controller"];
        return new[] { controllerName };
    });

    c.DocInclusionPredicate((docName, api) => true);

    // XML Dokümantasyonu ekleme
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }

    // JWT Authentication for Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Swagger UI'ı özelleştir
    c.EnableAnnotations();
    c.UseInlineDefinitionsForEnums();
    c.DescribeAllParametersInCamelCase();
});

// MediatR
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(CreateUserCommand).Assembly);
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
});

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();

// JWT Authentication
var jwtKey = configuration["Jwt:Key"];
if (string.IsNullOrWhiteSpace(jwtKey) || Encoding.UTF8.GetBytes(jwtKey).Length < 32)
{
    throw new Exception("JWT Secret Key must be at least 32 characters long.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// CORS Policy (Daha esnek hale getirildi)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy
                .AllowAnyOrigin() // Herkese açık hale getirildi
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestPerformanceMiddleware>();

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
