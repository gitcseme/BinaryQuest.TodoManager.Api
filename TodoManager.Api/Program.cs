using Microsoft.AspNetCore.Mvc;
using TodoManager.Api.Extensions;
using TodoManager.Api.Filters;
using TodoManager.Api.Middlewares;
using FluentValidation;
using FluentValidation.AspNetCore;
using TodoManager.Api.DtoValidators;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureCors();
builder.Services.ConfigureLogger();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureCookies();
builder.Services.RegisterRepositoryManagers();
builder.Services.RegisterServices();



builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
})
.AddFluentValidation(fv =>
{
    fv.ImplicitlyValidateChildProperties = true;
    fv.ImplicitlyValidateRootCollectionElements = true;
    fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
