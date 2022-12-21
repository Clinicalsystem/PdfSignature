using DowloadXmlPDF.ViewModels;

namespace DowloadXmlPDF.Views.Setting;

public partial class SettingPage : ContentPage
{
	public SettingPage(SettingViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}