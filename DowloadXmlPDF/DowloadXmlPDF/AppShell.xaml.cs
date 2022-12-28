using DowloadXmlPdf.Views.Home;
using DowloadXmlPdf.Views.Setting;

namespace DowloadXmlPdf;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute("HomePage", typeof(HomePage));
        Routing.RegisterRoute("SettingPage", typeof(SettingPage));
    }
}
