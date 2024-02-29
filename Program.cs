using CSS_Service.API.Modules;
using CSS_Service.Domain.Interfaces;
using CSS_Service.Domain.Queries;
using CSS_Service.Infrastructure.Models;
using CSS_Service.Infrastructure.Modules;
using CSS_Service.Infrastructure.Repositories;
using CSS_Service.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB CONTEXT DEPENDENCIES:
builder.Services.AddDbContextFactory<CssDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnectionString")),
    ServiceLifetime.Transient
);

// MediatR Dependency:
builder.Services.AddMediatR(
    cfg => cfg.RegisterServicesFromAssemblies(
        Assembly.GetExecutingAssembly(),
        typeof(GetAllIdentsHandler).Assembly
    )
);

// AUTOMAPPER DEPENDENCIES:
builder.Services.AddAutoMapper(configuration =>
{
    configuration.AddProfile(new AutoMapperProfile());
    configuration.AddProfile(new AutoMapperProfileApi());
});

// ADD DEPENDENCIES:
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IIdentRepository, IdentRepository>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();
builder.Services.AddScoped<ICitiesRepository, CitiesRepository>();
builder.Services.AddScoped<ISkladisteRepository, SkladisteRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IReferentRepository, ReferentRepository>();
builder.Services.AddScoped<IMasinaRepository, MasinaRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IContactPersonRepository, ContactPersonRepository>();
builder.Services.AddScoped<INarudzbinaRepository, NarudzbinaRepository>();
builder.Services.AddScoped<INarudzbinaItemRepository, NarudzbinaItemRepository>();
builder.Services.AddScoped<INarudzbinaService, NarudzbinaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
