using CRM.Models;

namespace CRM;
using CRM.Pages;
using System.Collections.ObjectModel;

public partial class CustomerPage : ContentPage
{
	private Controller c;
	private CustomersModel cm;


    public CustomerPage(Controller controller, CustomersModel cm)
	{
		InitializeComponent();
		c = controller;
		this.cm = cm;
        BindingContext = this;
    }

	public void AddCustomerButtonClicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new AddCustomerPage(c, cm));
    }

	public void UpdateCustomersList()
	{
        List<CustomerEntry> customers = new List<CustomerEntry>();
        foreach (KeyValuePair<string, Customer> c in cm.Customers)
        {
            CustomerEntry ce = new CustomerEntry();
            ce.Id = c.Key;
            ce.Customer = c.Value;
            customers.Add(ce);
        }
        Customers.ItemsSource = customers;
    }
    public void OnCustomerTapped(object sender, EventArgs e)
    {
        if (sender is Frame frame)
        {
            var tapGesture = frame.GestureRecognizers
                                  .OfType<TapGestureRecognizer>()
                                  .FirstOrDefault();

            if (tapGesture?.CommandParameter is CustomerEntry customerEntry)
            {
                Navigation.PushAsync(new CustomerViewPage(
                    new KeyValuePair<string, Customer>(customerEntry.Id, customerEntry.Customer),
                    c));
            }
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateCustomersList();
    }

    public class CustomerEntry
    {
        public string Id { get; set; }              // Firestore document ID (key)
        public Customer Customer { get; set; }      // Actual customer object

        public string Name => Customer?.Name;
        public string Email => Customer?.Email;
        public string Phone => Customer?.Phone;
    }
}