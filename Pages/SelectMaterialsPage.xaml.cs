using CRM.Models;

namespace CRM.Pages;

public partial class SelectMaterialsPage : ContentPage
{
	private Controller c;
	private MaterialsModel mm;
	private List<Material> _selectedMaterials;
    public void ChangeSelectedMaterials(Material material)
	{
		if(_selectedMaterials.Contains(material))
		{
			_selectedMaterials.Remove(material);
		}
		else
		{
			_selectedMaterials.Add(material);
        }
    }
	public void BackButtonClicked(object sender, EventArgs e)
	{
		Navigation.PopAsync();
    }
    public SelectMaterialsPage(Controller controller, MaterialsModel materialsModel)
	{
		InitializeComponent();
		c = controller;
		mm = materialsModel;
		BindingContext = this;
    }

	public void AddMaterialButtonClicked(object sender, EventArgs e)
	{
		
    }

	
}