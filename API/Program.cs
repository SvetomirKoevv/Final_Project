using System.Security.Claims;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidIssuer = "svetomir",
            ValidAudience = "plvduni",
            IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.ASCII.GetBytes("SuperSecretKeyThatNooneWillEverGuess123!")),
            RoleClaimType = ClaimTypes.Role,
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("❌ AUTH FAILED");
                Console.WriteLine(context.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("✅ TOKEN VALID");
                return Task.CompletedTask;
            },
            OnMessageReceived = context =>
            {
                Console.WriteLine("📩 TOKEN RECEIVED");
                Console.WriteLine(context.Token);
                return Task.CompletedTask;
            }
        };
    });
    
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();