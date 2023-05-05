using DataLibrary;
using DataLibrary.DbAccess;
using DataLibrary.DbServices;
using DataLibrary.AutoMapperProfiles;
//using DataLibrary.Services.SDATScrapers;
//using DataLibrary.Services.BlobService;
using AutoMapper;
using Serilog;

namespace GroundRentProcessor;

public class Program
{
    public static void Main(string[] args)
    {
        var logConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(logConfig)
            .CreateLogger();

        try
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            Log.Information("App start");
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(Log.Logger);
            builder.Host.UseSerilog();
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddScoped<IDataContext>(o => new DataContext(configuration.GetConnectionString("Default")));
            //builder.Services.AddSingleton<IBlobService>(o => new BlobService(configuration.GetConnectionString("AzureBlobStorage")));
            //var dockerHost = Environment.GetEnvironmentVariable("DB_HOST");
            //var dockerName = Environment.GetEnvironmentVariable("DB_NAME");
            //var dockerPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
            //builder.Services.AddScoped<IDataContext>(
            //    s => new DataContext(
            //        configuration.GetConnectionString(
            //            $"Data Source={dockerHost};Initial Catalog={dockerName};User ID=SA;Password={dockerPassword}")));
            builder.Services.AddScoped<ExceptionLogDataServiceFactory>();
            builder.Services.AddScoped<ALLEDataServiceFactory>();
            builder.Services.AddScoped<ANNEDataServiceFactory>();
            builder.Services.AddScoped<BACIDataServiceFactory>();
            builder.Services.AddScoped<BACODataServiceFactory>();
            builder.Services.AddScoped<CALVDataServiceFactory>();
            builder.Services.AddScoped<CARODataServiceFactory>();
            builder.Services.AddScoped<CARRDataServiceFactory>();
            builder.Services.AddScoped<CECIDataServiceFactory>();
            builder.Services.AddScoped<CHARDataServiceFactory>();
            builder.Services.AddScoped<DORCDataServiceFactory>();
            builder.Services.AddScoped<FREDDataServiceFactory>();
            builder.Services.AddScoped<GARRDataServiceFactory>();
            builder.Services.AddScoped<HARFDataServiceFactory>();
            builder.Services.AddScoped<HOWADataServiceFactory>();
            builder.Services.AddScoped<KENTDataServiceFactory>();
            builder.Services.AddScoped<MONTDataServiceFactory>();
            builder.Services.AddScoped<PRINDataServiceFactory>();
            builder.Services.AddScoped<QUEEDataServiceFactory>();
            builder.Services.AddScoped<SOMEDataServiceFactory>();
            builder.Services.AddScoped<STMADataServiceFactory>();
            builder.Services.AddScoped<TALBDataServiceFactory>();
            builder.Services.AddScoped<WASHDataServiceFactory>();
            builder.Services.AddScoped<WICODataServiceFactory>();
            builder.Services.AddScoped<WORCDataServiceFactory>();
            builder.Services.AddAutoMapper(typeof(AutoMapperEntryPoint).Assembly);
            var mapper = new MapperConfiguration(options =>
            {
                options.AddProfile<AddressProfile>();
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseSerilogRequestLogging();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "App start failure");
        }
        finally
        {
            Log.Information("App shut down complete");
            Log.CloseAndFlush();
        }
    }
}