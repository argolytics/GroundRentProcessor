using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;
using BlazorShared.Pages;
using DataLibrary.DbAccess;
using DataLibrary.Services.BlobService;
using Microsoft.Extensions.Configuration;
using DataLibrary.DbServices;
using DataLibrary.Services.SDATScrapers;
using DataLibrary;
using AutoMapper;
using DataLibrary.AutoMapperProfiles;
using BlazorShared;

namespace WinFormsUI
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            var services = new ServiceCollection();
            services.AddScoped<IDataContext>(o => new DataContext(Program.Configuration.GetConnectionString("Default")));
            services.AddSingleton<IBlobService>(o => new BlobService(Program.Configuration.GetConnectionString("AzureBlobStorage")));
            services.AddWindowsFormsBlazorWebView();
            services.AddBlazorWebViewDeveloperTools();
            services.AddScoped<ExceptionLogDataServiceFactory>();
            //builder.Services.AddScoped<AlleganyCountyDataServiceFactory>();
            //builder.Services.AddScoped<AnneArundelCountyDataServiceFactory>();
            services.AddScoped<BaltimoreCityDataServiceFactory>();
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
            services.AddScoped<IRealPropertySearchScraper, BaltimoreCityScraper>();
            services.AddAutoMapper(typeof(AutoMapperEntryPoint).Assembly);
            var mapper = new MapperConfiguration(options =>
            {
                options.AddProfile<AddressProfile>();
            });
            blazorWebView.HostPage = "wwwroot\\index.html";
            blazorWebView.Services = services.BuildServiceProvider();
            blazorWebView.RootComponents.Add<App>("#app");
        }

    }
}