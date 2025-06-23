namespace CRM
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(CalendarPage), typeof(CalendarPage));
            Routing.RegisterRoute(nameof(JobsPage), typeof(JobsPage));
            Routing.RegisterRoute(nameof(CustomerPage), typeof(CustomerPage));
        }
    }
}
