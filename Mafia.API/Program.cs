using System.Text;
using Mafia.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Mafia.Persistence;
using Mafia.Core.Interfaces;
using Mafia.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Mafia.Persistence.Repositories;
using Mafia.Infrastructre;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IJwtTokenProvider, JwtTokenProvider>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<IGameRegistrationService, GameRegistrationService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IProductService, ProductService>();


builder.Services.AddTransient<IUsersRepository, UsersRepository>();
builder.Services.AddTransient<IFileRepository, FileRepository>();
builder.Services.AddTransient<ICartRepository, CartRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IGameRegistrationRepository, GameRegistrationRepository>();
builder.Services.AddTransient<IPhotoRepository, PhotoRepository>();
builder.Services.AddTransient<IGameRepository, GameRepository>();
builder.Services.AddTransient<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Mafia API",
        Version = "v1",
        Description = "API для приложения Mafia"
    });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
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
            new string[] {}
        }
    });
});

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
//        if (context.Request.Cookies.ContainsKey("cooookies"))
//        {
//            context.Token = context.Request.Cookies["cooookies"];
//        }
//        return Task.CompletedTask;
//    }
//};
    
});

builder.Services.AddDirectoryBrowser(); //для myimages. потом отключить ОПАСНО!!!!


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mafia API v1");
        c.RoutePrefix = string.Empty; // Чтобы Swagger UI был доступен по корневому URL
    });
    app.UseDeveloperExceptionPage();
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
        var adminEmail = "admin@123.com";
        
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
