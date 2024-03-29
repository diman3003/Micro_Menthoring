using ScWorks.Api.Controllers;
using ScWorks.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
ServicesRegistration(builder.Services);
var app = builder.Build();
Configure(app);

new ScWorksController().Register(app);

app.Run();


void ServicesRegistration(IServiceCollection services)
{
    services.AddDbContext<ScWorksDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("Menthoring")));
    services.AddScoped<IScWorksRepo, ScWorksRepo>();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
}

void Configure(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}
