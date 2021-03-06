﻿using System;
using System.Reflection;

using Autofac.Extras.IocManager;

using JetBrains.Annotations;

using Stove.Redis;
using Stove.Redis.Configurations;
using Stove.Runtime.Caching;

namespace Stove
{
    public static class StoveRedisRegistrationExtensions
    {
        /// <summary>
        ///     Uses the stove redis caching.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="redisCacheConfigurer">The redis cache configurer.</param>
        /// <returns></returns>
        [NotNull]
        public static IIocBuilder UseStoveRedisCaching([NotNull] this IIocBuilder builder, [CanBeNull] Func<IStoveRedisCacheConfiguration, IStoveRedisCacheConfiguration> redisCacheConfigurer = null)
        {
            return builder.RegisterServices(r =>
            {
                r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
                r.Register<IStoveRedisCacheConfiguration, StoveRedisCacheConfiguration>(Lifetime.Singleton);
                r.RegisterType<StoveRedisCache>();
                r.Register<ICacheManager, StoveRedisCacheManager>();

                if (redisCacheConfigurer != null)
                {
                    r.Register(ctx => redisCacheConfigurer);
                }
            });
        }

        /// <summary>
        ///     Uses the typeless redis cache serializer. <seealso cref="DefaultRedisCacheSerializer" />
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        [NotNull]
        public static IIocBuilder UseTypelessRedisCacheSerializer([NotNull] this IIocBuilder builder)
        {
            return builder.RegisterServices(r => { r.Register<IRedisCacheSerializer, TypelessRedisCacheSerializer>(); });
        }
    }
}
