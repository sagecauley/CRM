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
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JEaF5cXmRCeUx3Rnxbf1x1ZFxMZFxbR3ZPIiBoS35Rc0VkWXlccXBVRWBUWEZwVEFd");
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
