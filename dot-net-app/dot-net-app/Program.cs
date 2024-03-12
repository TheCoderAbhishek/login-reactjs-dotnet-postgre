using dot_net_app.Data;
using dot_net_app.Interface.AccountInterface;
using dot_net_app.Interface.EmailServiceInterface;
using dot_net_app.Model.EmailsService;
using dot_net_app.Service.AccountService;
using dot_net_app.Service.EmailsService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

// Add services to the container.
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Add TempData
builder.Services.AddMvc()
    .AddSessionStateTempDataProvider();

builder.Services.AddSession();

// Add DbContext
builder.Services.AddDbContext<AccountDbContext>(options =>
    options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

// Email Sending
var smtpSettings = Configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
if (smtpSettings != null)
{
    builder.Services.AddSingleton(smtpSettings);
    builder.Services.AddTransient<IEmailService, EmailsService>();
}

// Add Memory Cache
builder.Services.AddMemoryCache();

// Add Interfaces
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    var audience = Configuration["Jwt:Audience"];
    var issuer = Configuration["Jwt:Issuer"];
    var key = Configuration["Jwt:Key"];

    if (audience != null && issuer != null && key != null)
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidIssuer = issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    }
    else
    {
        Console.WriteLine("One or more JWT configuration values are null.");
    }
});

var app = builder.Build();

app.UseRouting();

// Configure session middleware
app.UseSession();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure CORS to allow requests from the frontend origin
app.UseCors(builder =>
{
    builder.WithOrigins("http://localhost:3000")
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials();
});

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
