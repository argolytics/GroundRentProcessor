using Microsoft.Extensions.Configuration;
using Serilog;
using System.Configuration;

namespace WinFormsUI
{
    internal static class Program
    {
        public static IConfigurationRoot Configuration { get; private set; }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            var builder = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();
            ApplicationConfiguration.Initialize();
            Application.Run(new Main());
        }
    }
}