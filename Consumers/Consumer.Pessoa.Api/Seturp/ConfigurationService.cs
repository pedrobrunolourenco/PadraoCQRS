using Consumer.Pessoa.Api.Models;
using MassTransit;
using System.Reflection;

namespace Consumer.Pessoa.Api.Seturp
{
    public static class ConfigurationService
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            //services.AddMassTransit(m =>
            //{
            //    m.AddConsumers(Assembly.GetExecutingAssembly());
            //    m.UsingRabbitMq((ctx, cfg) =>
            //    {
            //        cfg.Host("localhost", "/", c =>
            //        {
            //            c.Username("pedrobrunorl");
            //            c.Password("Jucla@123");
            //        });

            //        cfg.ReceiveEndpoint("FilaPessoa", ep =>
            //        {
            //            ep.PrefetchCount = 10;
            //            ep.UseMessageRetry(r => r.Interval(2, 100));
            //            ep.ConfigureConsumer<PessoaModelMessage>(m);
            //        });

            //        cfg.ConfigureEndpoints(ctx);
            //    });
            //});
            services.AddMassTransit(x =>
            {
                x.AddConsumer<PessoaConsumer>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                        h.Username("pedrobrunorl");
                        h.Password("Jucla@123");
                    });
                    cfg.ReceiveEndpoint("FilaPessoa", ep =>
                    {
                        ep.PrefetchCount = 10;
                        ep.UseMessageRetry(r => r.Interval(2, 100));
                        ep.ConfigureConsumer<PessoaConsumer>(provider);
                    });
                }));
            });
            return services;
        }
    }
}
