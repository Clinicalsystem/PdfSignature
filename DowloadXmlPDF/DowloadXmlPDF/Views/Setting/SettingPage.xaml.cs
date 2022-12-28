using DowloadXmlPdf.Services;
using DowloadXmlPdf.ViewModels;

namespace DowloadXmlPdf.Views.Setting;

public partial class SettingPage : ContentPage
{
    private SettingViewModel _viewModel;

    public SettingPage()
    {
        BindingContext = _viewModel = new SettingViewModel(App.ServiceProvider.GetRequiredService<IOpenFactura>());
        InitializeComponent();
    }

    public SettingPage(SettingViewModel viewModel)
	{
		BindingContext = _viewModel = viewModel;
		InitializeComponent();
	}
}