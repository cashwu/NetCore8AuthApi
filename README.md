
## NET Core 8 Auth Api


- add EF Core package

```shell
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --prerelease
dotnet add package Microsoft.EntityFrameworkCore.Design --prerelease
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --prerelease
```

- add Authorization 

```csharp
builder.Services.AddAuthorization;
```

- create user class and db context class

```csharp
class MyUser : IdentityUser
{
}

class AppDbContext : IdentityDbContext<MyUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}
```

- add db context service 

```csharp
builder.Services.AddDbContext<AppDbContext>(a => a.UseSqlite("DataSource=app.db"));
```

- add identity service 

```csharp
builder.Services
       .AddIdentityApiEndpoints<MyUser>()
       .AddEntityFrameworkStores<AppDbContext>();
```

- add identity api

```csharp
app.MapIdentityApi<MyUser>();
```

- add Authorization middleware

```csharp
app.UseAuthorization(); 
```

- add test auth api

```csharp
app.MapGet("/user", (ClaimsPrincipal user) => $"Hello {user.Identity?.Name}")
   .RequireAuthorization();
```

- update ef tool

```shell
dotnet tool update --global dotnet-ef --prerelease
```

- ef migration

```shell
dotnet ef migrations add init
dotnet ef database update
```

- modify Swagger setting

```csharp
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
```
