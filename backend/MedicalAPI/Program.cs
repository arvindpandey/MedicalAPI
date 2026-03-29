using MedicalAPI.BusinessLogic;
using MedicalAPI.Interface;
using MedicalAPI.MedicalEntity;
 
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore; 
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models; 
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MedicalDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MedicalDBConnection")));

builder.Services.AddHttpClient("GroqClient", client =>
{
    var apiKey = builder.Configuration["Groq:ApiKey"];
    var baseUrl = builder.Configuration["Groq:BaseUrl"] ?? "https://api.groq.com/"; 

    client.BaseAddress = new Uri(baseUrl);
    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddScoped<ISymptomAIService, SymptomAIBL>();
builder.Services.AddScoped<IPatientService, PatientBL>();
builder.Services.AddScoped<IUserService, UserBL>();

//services cors
var devCorsPolicy = "devCorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(devCorsPolicy, builder => {
        builder.WithOrigins("http://localhost:3000/").AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");
        builder.SetIsOriginAllowed(origin => true);
    });
});


var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
          .AddJwtBearer(options =>
          {
              options.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateIssuer = true,
                  ValidateAudience = true,
                  ValidateLifetime = true,
                  ValidateIssuerSigningKey = true,
                  ValidIssuer = builder.Configuration[jwtKey],
                  ValidAudience = builder.Configuration["Jwt:Issuer"],
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
              };
          });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(opts => opts.AddPolicy(devCorsPolicy, policy => policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();


 
app.MapControllerRoute(
    name: "default", pattern: "{controller=User}/{action=GetAllUsers}/{id:int?}"
    );
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Homepathy Clinic API v1");

    });
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(devCorsPolicy);
app.UseAuthentication();
app.UseAuthorization(); 
app.UseHttpsRedirection(); 
app.MapControllers(); 
app.Run();
 