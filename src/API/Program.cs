using System.Text;
using API.Middlewares;
using Application;
using Application.IServices;
using Application.Services;
using AutoMapper;
using Common.CustomValidators;
using Common.RequestMessages;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence.Data;
using Persistence.IRepositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c => c.EnableAnnotations());
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Simple Bank API", Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
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
            new string[] { }
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("TokenKey"))),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped(typeof(IAccountRepository), typeof(AccountRepository));
builder.Services.AddScoped(typeof(IAccountHolderRepository), typeof(AccountHolderRepository));
builder.Services.AddScoped(typeof(IIBANStoreRepository), typeof(IBANStoreRepository));
builder.Services.AddScoped(typeof(ITransactionFeeRepository), typeof(TransactionFeeRepository));
builder.Services.AddScoped(typeof(ITransactionHistoryRepository), typeof(TransactionHistoryRepository));

//SQLite db
builder.Services.AddDbContext<ApplicationDbContext>();

//custom validator
builder.Services.AddControllers()
    .AddFluentValidation(c => c.RegisterValidatorsFromAssemblyContaining<AccountCreateValidator>());


//AutoMapper
builder.Services
    .AddAutoMapper(typeof(AutoMapperProfile).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// global error handler
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseAuthentication();
app.UseHttpsRedirection();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();