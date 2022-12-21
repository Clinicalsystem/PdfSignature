using MauiApp1.Views.Home;

namespace MauiApp1;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routes();
	}

	private void Routes()
	{
        Routing.RegisterRoute("HomePage", typeof(HomePage));
    }
}
