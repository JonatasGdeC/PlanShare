using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanShare.App.Navigation;

namespace PlanShare.App.ViewModels.Pages.User.Register;

public partial class RegisterUserViewModel : ViewModelBase
{
    [ObservableProperty] private Models.RegisterUser model;
    
    private readonly INavigationService _navigation;
    
    public RegisterUserViewModel(INavigationService navigationService)
    {
        Model = new Models.RegisterUser();
        _navigation = navigationService;
    }


    [RelayCommand]
    private async Task LoginWithEmailAndPasswords() => await _navigation.GoToAsync(state: $"../{RoutePages.LOGIN_PAGE}");
}