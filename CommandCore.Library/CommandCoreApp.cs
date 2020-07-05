using CommandCore.Library.Interfaces;
using CommandCore.LightIoC;
using IServiceProvider = CommandCore.LightIoC.IServiceProvider;

namespace CommandCore.Library
{
    /// <summary>
    /// CommandCoreApp is the entry point of the CommandCore library. It represents the CommandCore application
    /// and provides the necessary parsing and type finding functionality for internal use. You should start with this
    /// class to start using MVC capabilities.
    /// </summary>
    public class CommandCoreApp
    {
        public int Parse(string[] args)
        {
            var serviceProvider = new BasicServiceProvider();
            RegisterServices(serviceProvider);
            return serviceProvider.Resolve<ICommandCoreVerbRunner>().Run(args);
        }

        private void RegisterServices(IServiceProvider serviceProvider)
        {
            serviceProvider.Register<IHelpGenerator, HelpGenerator>();
            serviceProvider.Register<ICommandParser, CommandParser>();
            serviceProvider.Register<IVerbTypeFinder, VerbTypeFinder>();
            serviceProvider.Register<IOptionsParser, OptionsParser>();
            serviceProvider.Register<IEntryAssemblyProvider, BasicEntryAssemblyProvider>();
            serviceProvider.Register<ICommandCoreVerbRunner, CommandCoreVerbRunner>();
        }
    }
}