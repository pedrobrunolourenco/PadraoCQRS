using MassTransit;

namespace Lartech.Api.Setup
{


    public static class ConfigurationService
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host("localhost", "/", c =>
                    {
                        c.Username("pedrobrunorl");
                        c.Password("Jucla@123");
                    });
                    cfg.ConfigureEndpoints(ctx);
                });
            });
            return services;
        }
    }

}
