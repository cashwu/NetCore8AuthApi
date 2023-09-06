using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer",
                                  new OpenApiSecurityScheme
                                  {
                                      Name = "Authorization",
                                      In = ParameterLocation.Header,
                                      Type = SecuritySchemeType.Http,
                                      Scheme = "Bearer"
                                  });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Name = "Bearer",
                In = ParameterLocation.Header,
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddAuthorization();

builder.Services.AddDbContext<AppDbContext>(a => a.UseSqlite("DataSource=app.db"));

builder.Services
       .AddIdentityApiEndpoints<MyUser>()
       .AddEntityFrameworkStores<AppDbContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapIdentityApi<MyUser>();
app.MapGet("/", () => "OK");
app.MapGet("/user", (ClaimsPrincipal user) => $"Hello {user.Identity?.Name}")
   .RequireAuthorization();

app.Run();

class MyUser : IdentityUser
{
}

class AppDbContext : IdentityDbContext<MyUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}