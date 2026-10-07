using Microsoft.EntityFrameworkCore;
using DeskFlow.API.Data;
using DeskFlow.API.Repositories;
using DeskFlow.API.Services;
using DeskFlow.API.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    // Correção: o gerador de OpenAPI do .NET 10 descreve parâmetros inteiros de rota
    // (ex: {id}) com "type": ["integer", "string"], porque na URL eles chegam como texto.
    // O Swagger UI não entende esse tipo "duplo" e trava o "Try it out" com
    // "Required field is not provided", mesmo com um valor digitado.
    // Aqui simplificamos o schema de volta para "integer" puro, só para a exibição no Swagger.
    options.AddSchemaTransformer((schema, context, cancellationToken) =>
    {
        if (schema.Type is JsonSchemaType type &&
            type.HasFlag(JsonSchemaType.Integer) &&
            type.HasFlag(JsonSchemaType.String))
        {
            schema.Type = JsonSchemaType.Integer;
            schema.Pattern = null;
        }

        return Task.CompletedTask;
    });

    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Description = "Informe o token JWT no formato: Bearer {seu token}"
        };

        var securityRequirement = new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
        };

        foreach (var path in document.Paths.Values)
        {
            foreach (var operation in path.Operations.Values)
            {
                operation.Security ??= new List<OpenApiSecurityRequirement>();
                operation.Security.Add(securityRequirement);
            }
        }

        return Task.CompletedTask;
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityCore<IdentityUser>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<AppDbContext>();

var jwtKey = builder.Configuration["Jwt:Key"]!;

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
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IChamadoRepository, ChamadoRepository>();
builder.Services.AddScoped<IChamadoService, ChamadoService>();
builder.Services.AddScoped<IInteracaoRepository, InteracaoRepository>();
builder.Services.AddScoped<IInteracaoService, InteracaoService>();
builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "DeskFlow API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();