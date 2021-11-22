using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PostNotifier.Consumers;
using PostNotifier.Contracts;
using PostNotifier.Mapping;
using PostNotifier.Models;
using PostNotifier.Repositories;

namespace PostNotifier.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureDatabaseSettings(this IServiceCollection services, IConfiguration Configuration)
        {
            services.Configure<SubscriptionsDatabaseSettings>(
        Configuration.GetSection(nameof(SubscriptionsDatabaseSettings)));

            services.AddSingleton<ISubscriptionDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<SubscriptionsDatabaseSettings>>().Value);
        }

        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddSingleton<ISubscriptionRepository, SubscriptionRepository>();
        }

        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            var mapConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new SubscriptionProfile());

            });

            IMapper mapper = mapConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public static void ConfigureMassTransit(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<SubscriptionConsumer>();
                x.AddConsumer<UnsubscriptionConsumer>();
                x.AddConsumer<PostCreatingConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ReceiveEndpoint("subscription-queue", e =>
                    {
                        e.ConfigureConsumer<SubscriptionConsumer>(context);
                    });

                    cfg.ReceiveEndpoint("unsubscription-queue", e =>
                    {
                        e.ConfigureConsumer<UnsubscriptionConsumer>(context);
                    });

                    cfg.ReceiveEndpoint("newpost-queue", e =>
                    {
                        e.ConfigureConsumer<PostCreatingConsumer>(context);
                    });
                });
            });

            services.AddMassTransitHostedService();
        }
    }
}
