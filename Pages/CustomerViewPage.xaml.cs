using System.ComponentModel;

namespace CRM.Pages;

public partial class CustomerViewPage : ContentPage, INotifyPropertyChanged
{
	Controller c;
	private KeyValuePair<string, Customer> _customerData;

    public List<CustomerCategory> CustomerCategories { get; set; }
    private CustomerCategory _selectedCategory;
    public CustomerCategory SelectedCategory
    {         
        get => _selectedCategory;
        set
        {
            _selectedCategory = value;
            OnPropertyChanged(nameof(SelectedCategory));
        }
    }

    public List<ContactMethod> ContactMethods { get; set; }
    private ContactMethod _selectedContactMethod;
    public ContactMethod SelectedContactMethod
    {
        get => _selectedContactMethod;
        set
        {
            _selectedContactMethod = value;
            OnPropertyChanged(nameof(SelectedContactMethod));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public CustomerViewPage(KeyValuePair<string, Customer> cust, Controller con)
	{
		InitializeComponent();

        c = con;
        _customerData = cust;
        CustomerCategories = Enum.GetValues(typeof(CustomerCategory)).Cast<CustomerCategory>().ToList();
        ContactMethods = Enum.GetValues(typeof(ContactMethod)).Cast<ContactMethod>().ToList();

        SelectedContactMethod = cust.Value.PreferredContactMethod;
        SelectedCategory = cust.Value.Category;
        customerNameEntry.Text = cust.Value.Name;
        customerEmailEntry.Text = cust.Value.Email;
        customerPhoneEntry.Text = cust.Value.Phone;
        customerAddressEntry.Text = cust.Value.Address;
        customerNotesEntry.Text = cust.Value.Notes;

        BindingContext = this;
    }

	private async void OnDeleteButtonClicked(object sender, EventArgs e)
	{
		if (_customerData.Value != null)
		{
			bool result = await c.DeleteCustomer(_customerData.Key);
            if (result)
			{
                DisplayAlert("Success", "Customer deleted successfully.", "OK");
				Navigation.PopAsync();
			}
			else
			{
				DisplayAlert("Error", "Failed to delete customer.", "OK");
			}
		}
		else
		{
			DisplayAlert("Error", "Customer data is not available.", "OK");
		}
    }
	private async void OnEditButtonClicked(object sender, EventArgs e)
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
        if (await c.EditCustomer(_customerData.Key, customer))
        {
            DisplayAlert("Success", "Customer edited successfully.", "OK");
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
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}