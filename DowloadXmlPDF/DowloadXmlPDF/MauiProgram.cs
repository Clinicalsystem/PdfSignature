using DowloadXmlPDF.Services;
using DowloadXmlPDF.ViewModels;
using DowloadXmlPDF.Views.Home;
using Syncfusion.Maui.Core.Hosting;
using Syncfusion.Maui.DataGrid.Hosting;
using Syncfusion.Maui.ListView.Hosting;

namespace DowloadXmlPDF;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .RegisterAppServices()
            .ConfigureSyncfusion();
        return builder.Build();
    }
    public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services
            .AddSingleton<HomePage>()
            //ViewModels
            .AddSingleton<SettingViewModel>()
            .AddSingleton<HomeViewModel>()
            //Services
            .AddSingleton<IOpenFactura, OpenFactura>();

        return mauiAppBuilder;
    }
    public static MauiAppBuilder ConfigureSyncfusion(this MauiAppBuilder mauiAppBuilder)
    {

        //ListView
        mauiAppBuilder.ConfigureSyncfusionListView();
        //DataGrip
        mauiAppBuilder.ConfigureSyncfusionCore();
        mauiAppBuilder.ConfigureSyncfusionDataGrid();
        //progresbar

        return mauiAppBuilder;
    }
}
