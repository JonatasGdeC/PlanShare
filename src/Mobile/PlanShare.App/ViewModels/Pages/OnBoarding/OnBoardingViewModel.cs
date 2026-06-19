using CommunityToolkit.Mvvm.Input;
using PlanShare.App.Navigation;

namespace PlanShare.App.ViewModels.Pages.OnBoarding;

public partial class OnBoardingViewModel
{
    [RelayCommand]
    private async Task LoginWithEmailAndPasswords() => await Shell.Current.GoToAsync(state: RoutePages.LOGIN_PAGE);

    [RelayCommand]
    private void LoginWithGoogle()
    {

    }

    [RelayCommand]
    private async Task RegisterUser() => await Shell.Current.GoToAsync(state: RoutePages.USER_REGISTER_PAGE);
}