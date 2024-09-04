using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Solid.Auth.Models;
using Solid.Auth.Models.CQRS.Commands;
using Solid.Auth.Models.CQRS.Handlers;
using Solid.Auth.Models.CQRS.Queries;
using Solid.Auth.Services.Implementation;
using Solid.Auth.Services.Repository;
using Solid.Core.Services.Implementation;
using Solid.Core.Services.Repository;
using Solid.Core.Services.Repository.CQRS;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Your other services...

var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:Secret"]);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero // Optional: Set ClockSkew to zero if tokens are time-sensitive
    };
});

builder.Services.AddScoped<IDMLServices>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new DMLServices(connectionString);
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<ICommandHandler<AddUserCommand>, AddUserCommandHandler>();
builder.Services.AddScoped<ICommandHandler<LoginUserCommand>, LoginUserCommandHandler>();
builder.Services.AddScoped<IQueryHandler<GetUserByEmailQuery, UserModel>, GetUserByEmailQueryHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
