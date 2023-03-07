using DataLibrary;
using DataLibrary.DbAccess;
using DataLibrary.DbServices;
using DataLibrary.AutoMapperProfiles;
using DataLibrary.Services.SDATScrapers;
using DataLibrary.Services.BlobService;
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
            builder.Services.AddSingleton<IBlobService>(o => new BlobService(configuration.GetConnectionString("AzureBlobStorage")));
            //var dockerHost = Environment.GetEnvironmentVariable("DB_HOST");
            //var dockerName = Environment.GetEnvironmentVariable("DB_NAME");
            //var dockerPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
            //builder.Services.AddScoped<IDataContext>(
            //    s => new DataContext(
            //        configuration.GetConnectionString(
            //            $"Data Source={dockerHost};Initial Catalog={dockerName};User ID=SA;Password={dockerPassword}")));
            builder.Services.AddScoped<ExceptionLogDataServiceFactory>();
            //builder.Services.AddScoped<AlleganyCountyDataServiceFactory>();
            //builder.Services.AddScoped<AnneArundelCountyDataServiceFactory>();
            builder.Services.AddScoped<BaltimoreCityDataServiceFactory>();
            //builder.Services.AddScoped<BaltimoreCountyDataServiceFactory>();
            //builder.Services.AddScoped<CalvertCountyDataServiceFactory>();
            //builder.Services.AddScoped<CarolineCountyDataServiceFactory>();
            //builder.Services.AddScoped<CarrollCountyDataServiceFactory>();
            //builder.Services.AddScoped<CecilCountyDataServiceFactory>();
            //builder.Services.AddScoped<CharlesCountyDataServiceFactory>();
            //builder.Services.AddScoped<DorchesterCountyDataServiceFactory>();
            //builder.Services.AddScoped<FrederickCountyDataServiceFactory>();
            //builder.Services.AddScoped<GarrettCountyDataServiceFactory>();
            //builder.Services.AddScoped<HarfordCountyDataServiceFactory>();
            //builder.Services.AddScoped<HowardCountyDataServiceFactory>();
            //builder.Services.AddScoped<KentCountyDataServiceFactory>();
            //builder.Services.AddScoped<MontgomeryCountyDataServiceFactory>();
            //builder.Services.AddScoped<PrinceGeorgesCountyDataServiceFactory>();
            //builder.Services.AddScoped<QueenAnnesCountyDataServiceFactory>();
            //builder.Services.AddScoped<SomersetCountyDataServiceFactory>();
            //builder.Services.AddScoped<StMarysCountyDataServiceFactory>();
            //builder.Services.AddScoped<TalbotCountyDataServiceFactory>();
            //builder.Services.AddScoped<WashingtonCountyDataServiceFactory>();
            //builder.Services.AddScoped<WicomicoCountyDataServiceFactory>();
            //builder.Services.AddScoped<WorcesterCountyDataServiceFactory>();
            builder.Services.AddScoped<IRealPropertySearchScraper, BaltimoreCityScraper>();
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