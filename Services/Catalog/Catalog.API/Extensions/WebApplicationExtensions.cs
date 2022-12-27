using Microsoft.EntityFrameworkCore;
using Npgsql;
using Polly;

namespace Catalog.API.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication MigrateDatabase<TContext>(this WebApplication app, Action<TContext, IServiceProvider> seeder) 
            where TContext : DbContext
        {
            using var scope = app.Services.CreateScope();
            
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetService<TContext>();

            try
            {
                logger.LogInformation("[Migration] Migration started", typeof(TContext).Name);

                var retry = Policy.Handle<NpgsqlException>()
                    .WaitAndRetry(new TimeSpan[]
                    {
                            TimeSpan.FromSeconds(3),
                            TimeSpan.FromSeconds(5),
                            TimeSpan.FromSeconds(8),
                    });

                retry.Execute(() =>
                {
                    context.Database.Migrate();
                    seeder(context, services);
                });

                logger.LogInformation("[Migration] Migration completed", typeof(TContext).Name);
            }
            catch (NpgsqlException ex)
            {
                logger.LogError(ex, "[Migration] An error occurred while migrating the database", typeof(TContext).Name);
            }
            return app;
        }
    }
}