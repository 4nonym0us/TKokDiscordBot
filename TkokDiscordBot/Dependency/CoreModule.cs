using Autofac;
using Config.Net;
using TkokDiscordBot.Configuration;
using TkokDiscordBot.Core;
using TkokDiscordBot.Data;
using TkokDiscordBot.Data.Abstractions;
using TkokDiscordBot.EntGaming;

namespace TkokDiscordBot.Dependency
{
    internal class CoreModule :Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PasteBinItemsLoader>().As<IItemsLoader>().InstancePerDependency(); //PasteBinItemsLoader|LocalFileItemsLoader
            builder.RegisterType<ItemsStore>().As<IItemsStore>().SingleInstance();
            builder.RegisterType<ItemsRepository>().As<IItemsRepository>().InstancePerDependency();

            builder.RegisterType<EntClient>().AsSelf().SingleInstance();
            builder.RegisterType<Bot>().AsSelf().SingleInstance();
            builder.RegisterType<CurrentGameStore>().AsSelf().SingleInstance();
            builder.RegisterType<GameStatusUpdater>().AsSelf().SingleInstance();

            var settings = new ConfigurationBuilder<ISettings>()
                .UseAppConfig()
                .UseJsonFile("user-secrets.json")
                .Build();

            builder.Register(c => settings).As<ISettings>().SingleInstance();
        }
    }
}
