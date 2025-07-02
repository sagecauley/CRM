using System.Runtime.CompilerServices;

namespace CRM;

public partial class JobsPage : ContentPage
{
	private Controller c;
    public JobsPage(Controller c)
	{
		InitializeComponent();
		this.c = c;
	}
	private async Task<AsyncVoidMethodBuilder> AddJobButtonClicked(object sender, EventArgs e)
	{
		await c.AddJob();
    }
}