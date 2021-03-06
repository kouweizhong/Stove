﻿using System;
using System.Reflection;

using Autofac;
using Autofac.Extras.IocManager;

using GreenPipes;

using JetBrains.Annotations;

using MassTransit;
using MassTransit.RabbitMqTransport;

using Stove.MQ;
using Stove.RabbitMQ;

namespace Stove
{
    public static class StoveRabbitMQRegistrationExtensions
    {
        /// <summary>
        ///     Uses the stove rabbit mq.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="rabbitMQConfigurer">The rabbit mq configurer.</param>
        /// <returns></returns>
        [NotNull]
        public static IIocBuilder UseStoveRabbitMQ([NotNull] this IIocBuilder builder, [NotNull] Func<IStoveRabbitMQConfiguration, IStoveRabbitMQConfiguration> rabbitMQConfigurer)
        {
            Check.NotNull(rabbitMQConfigurer, nameof(rabbitMQConfigurer));

            builder
                .RegisterServices(r =>
                {
                    r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
                    r.Register<IStoveRabbitMQConfiguration, StoveRabbitMQConfiguration>(Lifetime.Singleton);
                    r.Register<IMessageBus, StoveRabbitMQMessageBus>();
                    r.Register(ctx => rabbitMQConfigurer);
                });

            builder.RegisterServices(r => r.UseBuilder(cb =>
            {
                cb.Register(ctx =>
                  {
                      var configuration = ctx.Resolve<IStoveRabbitMQConfiguration>();

                      IBusControl busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
                      {
                          IRabbitMqHost host = cfg.Host(new Uri(configuration.HostAddress), h =>
                          {
                              h.Username(configuration.Username);
                              h.Password(configuration.Password);
                          });

                          if (configuration.UseRetryMechanism)
                          {
                              cfg.UseRetry(rtryConf => { rtryConf.Immediate(configuration.MaxRetryCount); });
                          }

                          cfg.ReceiveEndpoint(host, configuration.QueueName, ec => { ec.LoadFrom(ctx); });
                      });

                      return busControl;
                  }).SingleInstance()
                  .As<IBusControl>()
                  .As<IBus>();
            }));

            return builder;
        }
    }
}
