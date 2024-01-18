using API.Scheduler;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder();

builder.Host.UseSerilog();

builder.Services.AddResponseCompression();
builder.Services.AddJsonStringLocalizer();
builder.Services.AddHashService();
builder.Services.AddJwtService();
builder.Services.AddAuthorization().AddAuthentication().AddJwtBearer();
builder.Services.AddContext<Context>(options => options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(Context))));
builder.Services.AddClassesMatchingInterfaces(nameof(API));
builder.Services.AddMediator(nameof(API));
builder.Services.AddSwaggerDefault();
builder.Services.AddControllers().AddJsonOptions();
builder.Services.AddSingleton<ISchedulerService, SchedulerService>();
builder.Services.AddSingleton<ICronJobRepository, CronJobRepository>();
builder.Services.AddSingleton<ICronJobService, CronJobService>();
builder.Services.AddSingleton<Context>();
builder.Services.AddSingleton<IUnitOfWork, UnitOfWork<Context>>();

var application = builder.Build();

//needed for running on starup -> change to other file
using (var scope = application.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var schedulerService = services.GetRequiredService<ISchedulerService>();
        await schedulerService.InitializeAsync();
    }
    catch (Exception ex)
    {
        // Trate a exceção conforme necessário
        Console.WriteLine("Erro durante a inicialização: " + ex.Message);
    }
}

application.UseException();
application.UseHsts().UseHttpsRedirection();
application.UseLocalization("en", "pt");
application.UseResponseCompression();
application.UseStaticFiles();
application.UseSwagger().UseSwaggerUI();
application.UseRouting();
application.MapControllers();
application.MapFallbackToFile("index.html");
application.Run();


