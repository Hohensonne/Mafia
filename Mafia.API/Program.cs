using System.Text;
using Mafia.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Mafia.Persistence;
using Mafia.Core.Interfaces;
using Mafia.Application.Servises;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Mafia.Persistence.Repositories;
using Mafia.Infrastructre;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IJwtTokenProvider, JwtTokenProvider>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddTransient<IUsersRepository, UsersRepository>();
builder.Services.AddTransient<IFileRepository, FileRepository>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});




builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = true;
});


var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
//    options.Events = new JwtBearerEvents
//{
//    OnMessageReceived = context =>
//    {
//        // ���������, ���� �� ����� � �����
//        if (context.Request.Cookies.ContainsKey("cooookies"))
//        {
//            context.Token = context.Request.Cookies["cooookies"];
//        }
//        return Task.CompletedTask;
//    }
//};
    
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mafia API v1");
        c.RoutePrefix = string.Empty; // Чтобы Swagger UI был доступен по корневому URL
    });
}



app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Добавляем инициализацию ролей
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    
    
        // Применяем миграции
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
        
        // Создаем администратора, если его нет
        var userManager = services.GetRequiredService<UserManager<User>>();
        var adminEmail = "admin@example.com";
        
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };
            
            var result = await userManager.CreateAsync(adminUser, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    
    
}

app.Run();
