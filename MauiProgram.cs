using CRM.Firebase;
using CRM.Models;
using CRM.Pages;
using Microsoft.Extensions.Logging;

namespace CRM
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
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
