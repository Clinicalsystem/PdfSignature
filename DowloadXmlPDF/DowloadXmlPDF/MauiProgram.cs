using CommunityToolkit.Maui;
using DowloadXmlPdf.Services;
using DowloadXmlPdf.ViewModels;
using DowloadXmlPdf.Views.Home;
using Syncfusion.Maui.Core.Hosting;

namespace DowloadXmlPdf;

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
			.UseMauiCommunityToolkit()
            .RegisterAppServices()
            .ConfigureSyncfusionCore();

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
            .AddSingleton<IMessageService, MessageService>()
            .AddSingleton<IOpenFactura, OpenFactura>();
        

        return mauiAppBuilder;
    }
}
