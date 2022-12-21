using MauiApp1.ViewModels;

namespace MauiApp1.Views.Setting;

public partial class SettingPage : ContentPage
{
	public SettingPage(SettingViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}