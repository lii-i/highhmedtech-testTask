using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    public static void Main()
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder();

        builder.Services.AddDbContext<MedTechDbContext>(options =>
            options.UseNpgsql(builder.Configuration["ConnectionStrings:PostgreSQL"])
        );

        WebApplication app = builder.Build();

        app.Run();
    }
}
