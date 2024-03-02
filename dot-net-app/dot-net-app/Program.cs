using dot_net_app.Data;
using dot_net_app.Interface.AccountInterface;
using dot_net_app.Interface.EmailServiceInterface;
using dot_net_app.Model.EmailsService;
using dot_net_app.Service.AccountService;
using dot_net_app.Service.EmailsService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

// Add services to the container.

// Add TempData
builder.Services.AddMvc()
    .AddSessionStateTempDataProvider();

builder.Services.AddSession();

// Add DbContext
builder.Services.AddDbContext<AccountDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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

app.UseAuthorization();

app.MapControllers();

app.Run();
