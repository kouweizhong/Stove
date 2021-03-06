﻿using Autofac.Extras.IocManager;

using Hangfire;

using Stove.Configuration;

namespace Stove.Configurations
{
    public class StoveHangfireConfiguration : IStoveHangfireConfiguration, ISingletonDependency
    {
        public StoveHangfireConfiguration(IStoveStartupConfiguration configuration)
        {
            Configuration = configuration;
        }

        public BackgroundJobServer Server { get; set; }

        public IGlobalConfiguration GlobalConfiguration => global::Hangfire.GlobalConfiguration.Configuration;

        public IStoveStartupConfiguration Configuration { get; }
    }
}
