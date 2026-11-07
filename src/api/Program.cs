using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static void Main()
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder();

        builder.AddServiceDefaults();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        builder.AddNpgsqlDbContext<MedTechDbContext>("medtech");


        builder.Services.AddScoped<PatientRepository>();
        builder.Services.AddScoped<PatientService>();
        builder.Services.AddScoped<DoctorRepository>();
        builder.Services.AddScoped<DoctorService>();
        builder.Services.AddScoped<DiseaseRepository>();
        builder.Services.AddScoped<DiseaseService>();

        WebApplication app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<MedTechDbContext>();
            db.Database.Migrate();
        }

        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapControllers();

        app.Run();
    }
}
