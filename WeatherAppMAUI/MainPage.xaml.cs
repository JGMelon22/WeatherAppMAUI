using WeatherAppMAUI.ViewModels;

namespace WeatherAppMAUI;

public partial class MainPage : ContentPage
{
    public MainPage(WeatherViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}