using CRM.Models;
using CRM.Pages;
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
        BindingContext = this;
	}
    private void AddJobButtonClicked(object sender, EventArgs e)
	{
        Navigation.PushAsync(new AddJobPage(c, _customerModel));
    }
}