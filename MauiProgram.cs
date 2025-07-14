using CRM.Firebase;
using CRM.Models;
using CRM.Pages;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;

namespace CRM
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mzk1MTA4MEAzMzMwMmUzMDJlMzAzYjMzMzAzYmhHQXBueGNiS0NUS2pJRW1Ld2ZMc3I4THV0dks3Y2I3azRCNXFZRGhrVDg9");
            var builder = MauiApp.CreateBuilder();

            builder.Services.AddSingleton<FirebaseAuthService>();
            builder.Services.AddSingleton<Controller>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<CalendarPage>();
            builder.Services.AddTransient<JobsPage>();
            builder.Services.AddTransient<CustomerPage>();
            builder.Services.AddSingleton<FirebaseFirestore>();
            builder.Services.AddSingleton<CustomersModel>();
            builder.Services.AddSingleton<JobsModel>();
            builder.Services.AddTransient<AddCustomerPage>();

            builder
                .UseMauiApp<App>()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
