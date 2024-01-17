using API.Application;
using API.Database;
using API.Worker;
using DotNetCore.EntityFrameworkCore;
using DotNetCore.Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHostedService<CronJobRun>();
builder.Services.AddSingleton<ICronJobService, CronJobService>();
builder.Services.AddSingleton<ICronJobRepository, CronJobRepository>();

builder.Services.AddContext<Context>(options => options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(Context))));
builder.Services.AddMediator(nameof(API));
builder.Build().Run();

