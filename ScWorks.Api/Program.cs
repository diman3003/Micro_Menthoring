using ScWorks.Api.APIs;
using ScWorks.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
ServicesRegistration(builder.Services);

var app = builder.Build();
new ScWorksController().Register(app);

Configure(app);
app.Run();

void ServicesRegistration(IServiceCollection services)
{
    services.AddDbContext<ScWorksDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("Infociti")));
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
