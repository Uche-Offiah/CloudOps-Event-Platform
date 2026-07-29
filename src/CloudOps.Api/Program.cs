using CloudOps.Api.Extensions;
using CloudOps.Application.DependencyInjection;
using CloudOps.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapApi();
app.Run();
