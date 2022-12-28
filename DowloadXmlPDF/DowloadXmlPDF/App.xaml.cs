using DowloadXmlPdf.Views.Home;

namespace DowloadXmlPdf;

public partial class App : Application
{

    public App(IServiceProvider sp)
    {
        InitializeComponent();
        ServiceProvider = sp;

        MainPage = new AppShell();
    }

    public static IServiceProvider ServiceProvider { get; set; }
}
