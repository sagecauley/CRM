namespace CRM.Pages;
using CRM.Models;

public partial class AddCustomerPage : ContentPage
{
    private Controller c;
    private CustomersModel cm;

    public List<CustomerCategory> CustomerCategories { get; set; }
    public CustomerCategory SelectedCategory { get; set; }

    public List<ContactMethod> ContactMethods { get; set; }
    public ContactMethod SelectedContactMethod { get; set; }
    public AddCustomerPage(Controller controller, CustomersModel cm)
	{
        InitializeComponent();
        c = controller;
        this.cm = cm;

        CustomerCategories = Enum.GetValues(typeof(CustomerCategory)).Cast<CustomerCategory>().ToList();
        ContactMethods = Enum.GetValues(typeof(ContactMethod)).Cast<ContactMethod>().ToList();

        BindingContext = this;
    }

    public async void AddCustomerButtonClicked(object sender, EventArgs e)
    {
        var customer = new Customer(
            customerNameEntry.Text,
            customerEmailEntry.Text,
            customerPhoneEntry.Text,
            customerAddressEntry.Text,
            customerNotesEntry.Text,
            SelectedCategory,
            SelectedContactMethod
        );
        if (await c.AddCustomer(customer))
        {
            DisplayAlert("Success", "Customer added successfully.", "OK");
            Navigation.PopAsync();
        }
        else
        {
            DisplayAlert("Error", "Failed to add customer. Please try again.", "OK");
        }
    }

    private void BackButtonClicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}