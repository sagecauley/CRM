using CRM.Models;
using System.Threading.Tasks;

namespace CRM;

public partial class ViewJobPage : ContentPage
{
	private CustomersModel _customerModel;
	private JobsModel _jobModel;
	private Controller _controller;
	private string _jobId;

	public List<JobStatus> JobStatus { get; set; }
	public JobStatus SelectedJobStatus { get; set; }

	public DateTime StartJobDate { get; set; }

	public TimeSpan StartJobTime { get; set; }

	public List<Customer> Customers { get; set; }

	public Customer SelectedCustomer { get; set; }
	public ViewJobPage(string jobId, Controller c, CustomersModel cm, JobsModel jm)
	{
		InitializeComponent();
		_jobId = jobId;
		_controller = c;
		_customerModel = cm;
		_jobModel = jm;

		LoadJobDetails();
		BindingContext = this;
	}

	private void BackButtonClicked(object sender, EventArgs e)
	{
		Navigation.PopAsync();
	}

	private void LoadJobDetails()
	{
		JobStatus = Enum.GetValues(typeof(JobStatus)).Cast<JobStatus>().ToList();
		Customers = _customerModel.Customers.Values.ToList();
		if (_jobModel.Jobs.TryGetValue(_jobId, out var job))
		{
			jobNameEntry.Text = job.Name;
			jobDescriptionEntry.Text = job.Description;
			jobCostEntry.Text = job.Cost;
			StartJobDate = job.StartDate.Date;
			StartJobTime = job.StartDate.TimeOfDay;
			SelectedJobStatus = job.Status;
			SelectedCustomer = _customerModel.Customers[job.CustomerId];
		}
		else
		{
			DisplayAlert("Error", "Job not found.", "OK");
			Navigation.PopAsync();
		}
	}

	private async void EditJobButtonClicked(object sender, EventArgs e)
	{
		DateTime jobDateTime = StartJobDate.Date + StartJobTime;
		Job job = new Job(jobNameEntry.Text, jobDescriptionEntry.Text, jobDateTime, jobCostEntry.Text, SelectedJobStatus, SelectedCustomer.Id)
		{
			Id = _jobId // Ensure the ID is set for updating
		};
		bool success = await _controller.EditJob(_jobId, job);
		if (success)
		{
			DisplayAlert("Success", "Job updated successfully.", "OK");
			Navigation.PopAsync();
		}
		else
		{
			DisplayAlert("Error", "Failed to update job.", "OK");
		}
	}

	private async void DeleteJobButtonClicked(object sender, EventArgs e)
	{
		_controller.DeleteJob(_jobId);
		bool success = await _controller.DeleteJob(_jobId);
		if (success)
		{
			DisplayAlert("Success", "Job deleted successfully.", "OK");
			Navigation.PopAsync();
		}
		else
		{
			DisplayAlert("Error", "Failed to delete job.", "OK");
		}
	}
}
		