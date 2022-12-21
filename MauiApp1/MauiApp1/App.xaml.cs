﻿using MauiApp1.Views.Home;
using MauiApp1.Views.Setting;

namespace MauiApp1;

public partial class App : Application
{
    public static INavigation GlobalNavigation { get; set; }
    public static IServiceProvider ServiceProvider { get; set; }
    private NavigationPage navigationPage { get; set; }
    public App(IServiceProvider sp, HomePage home)
	{
		InitializeComponent();
        ServiceProvider = sp;
        navigationPage = new NavigationPage(home);
        GlobalNavigation = navigationPage.Navigation;

        MainPage = navigationPage;
	}
}
