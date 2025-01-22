using CssService.API.Middlewares;
using CssService.API.Modules;
using CssService.Domain.Interfaces;
using CssService.Domain.Interfaces.ExternalServices;
using CssService.Domain.Queries;
using CssService.Infrastructure.Models;
using CssService.Infrastructure.Modules;
using CssService.Infrastructure.Repositories;
using CssService.Infrastructure.Repositories.ExternalServicesRepository;
using CssService.Infrastructure.Transactions;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.Converters.Add(new JsonTimeConverter("yyyy-MM-ddTHH:mm:ss"));
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AUTOMAPPER DEPENDENCIES:
builder.Services.AddAutoMapper(configuration =>
{
    configuration.AddProfile(new AutoMapperModule());
    configuration.AddProfile(new AutoMapperProfileApi());
});

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

// REPOSITORY DEPENDENCIES:
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IIdentRepository, IdentRepository>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ISkladisteRepository, SkladisteRepository>();
builder.Services.AddScoped<IReferentRepository, ReferentRepository>();
builder.Services.AddScoped<IMasinaRepository, MasinaRepository>();
builder.Services.AddScoped<INarudzbinaRepository, NarudzbinaRepository>();
builder.Services.AddScoped<IContactPersonRepository, ContactPersonRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
builder.Services.AddScoped<ISignatureRepository, SignatureRepository>();
builder.Services.AddScoped<IPdfRepository, PdfRepository>();
builder.Services.AddScoped<IEmailRepository, EmailRepository>();

builder.Services.AddScoped<IUnitOfWorkRepository, UnitOfWorkRepository>();

// MediatR Dependency:
builder.Services.AddMediatR(
    cfg => cfg.RegisterServicesFromAssemblies(
        Assembly.GetExecutingAssembly(),
        typeof(GetUsersHandler).Assembly
    )
);

// Dapper register
builder.Services.AddSingleton<DapperContext>();

// Unit of Work (Transactions) register
builder.Services.AddScoped<TransactionManagerService>();

builder.Services.AddMemoryCache();

builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.Configure(context.Configuration.GetSection("Kestrel"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

// Exception handling middleware.
app.UseMiddleware<CustomExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();



app.Run();
