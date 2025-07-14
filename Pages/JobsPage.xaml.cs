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

        JobsScheduler.View = SchedulerView.Week;

        BindingContext = this;

	}
    private async void AddJobButtonClicked(object sender, EventArgs e)
	{
        AddJobPage addJobPage = new AddJobPage(c, _customerModel);
        await Navigation.PushAsync(addJobPage);
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
                Id = job.Id,
                IsAllDay = false
            });
        }

        JobsScheduler.AppointmentsSource = appointments;
    }

    private async void OnAppointmentTapped(object sender, SchedulerTappedEventArgs e)
    {
        if (e.Appointments != null)
        {
            if (e.Appointments[0] is SchedulerAppointment appointment)
            {
                {
                    if (appointment.Id is string id)
                    {
                        await Navigation.PushAsync(new ViewJobPage(id, c, _customerModel, _jobModel));
                    }
                }
            }
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadAppointments();
    }
}