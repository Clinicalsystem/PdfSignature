using DowloadXmlPdf.Models.OF;
using DowloadXmlPdf.ViewModels;
using System.Collections.ObjectModel;

namespace DowloadXmlPdf.Views.Home;

public partial class HomePage : ContentPage
{
    private HomeViewModel homeView;
    public HomePage(HomeViewModel viewModel)
    {

        BindingContext = homeView = viewModel;
        InitializeComponent();

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    private void SelectAll_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (homeView.DteLists.Count > 0)
        {
            var x = dataGrid.Width;
            var list = new List<Data>(homeView.DteLists);

            foreach (var item in list)
            {
                item.IsSelected = e.Value;
            }
            homeView.IsVisibleDowload = true;
            homeView.DteLists = new ObservableCollection<Data>(list);

        }
        else
        {
            SelectAll.IsChecked = false;
        }



    }

    private void Search_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.NewTextValue))
        {
            homeView.SearchCommand.Execute(e.NewTextValue);
        }
    }
}