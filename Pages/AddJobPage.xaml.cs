using CRM.Models;

namespace CRM.Pages;

public partial class AddJobPage : ContentPage
{
	private Controller c;
	private CustomersModel cm;

	public List<JobStatus> JobStatus { get; set; }
	public JobStatus SelectedJobStatus { get; set; }

    public DateTime StartJobDate { get; set; } = DateTime.Today;

	public TimeSpan StartJobTime { get; set; } = DateTime.Now.TimeOfDay;

    public List<Customer> Customers { get; set; }

	public Customer SelectedCustomer { get; set; }

    public AddJobPage(Controller c, CustomersModel cm)
	{
		InitializeComponent();
		this.c = c;
		this.cm = cm;
        JobStatus = Enum.GetValues(typeof(JobStatus)).Cast<JobStatus>().ToList();

		Customers = cm.Customers.Values.ToList();
        BindingContext = this;
    }

	private void BackButtonClicked(object sender, EventArgs e)
	{
		Navigation.PopAsync();
    }

	private async void AddJobButtonClicked(object sender, EventArgs e)
	{
		DateTime jobDateTime = StartJobDate.Date + StartJobTime;
		Job job = new Job(jobNameEntry.Text, jobDescriptionEntry.Text, jobDateTime, jobCostEntry.Text, SelectedJobStatus, SelectedCustomer.Id);
		bool success = await c.AddJob(job);
		if (success)
		{
			DisplayAlert("Success", "Job added successfully.", "OK");
			Navigation.PopAsync();
		}
		else
		{
			await DisplayAlert("Error", "Failed to add job.", "OK");
		}
	}
}