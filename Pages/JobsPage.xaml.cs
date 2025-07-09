using CRM.Models;
using CRM.Pages;
using Syncfusion.Maui.Calendar;
using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace CRM;

public partial class JobsPage : ContentPage
{
	private Controller c;
	private JobsModel _jobModel;
	private CustomersModel _customerModel;

    public JobsPage(Controller c, JobsModel jm, CustomersModel cm)
	{
		InitializeComponent();
		this.c = c;
		_jobModel = jm;
		_customerModel = cm;

        JobsScheduler.View = SchedulerView.Month;

        //LoadAppointments();
        BindingContext = this;

	}
    private void AddJobButtonClicked(object sender, EventArgs e)
	{
        Navigation.PushAsync(new AddJobPage(c, _customerModel));
    }


    private void LoadAppointments()
    {
        var appointments = new ObservableCollection<SchedulerAppointment>();

        foreach (var job in _jobModel.Jobs.Values)
        {
            appointments.Add(new SchedulerAppointment
            {
                StartTime = job.StartDate,
                EndTime = job.StartDate.AddHours(1),
                Subject = job.Name,
                Notes = job.Description,
                IsAllDay = false
            });
        }

        JobsScheduler.AppointmentsSource = appointments;
    }
}