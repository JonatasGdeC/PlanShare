using CommunityToolkit.Mvvm.Input;
using PlanShare.App.Navigation;

namespace PlanShare.App.ViewModels.Pages.OnBoarding;

public partial class OnBoardingViewModel(INavigationService navigationService) : ViewModelBase
{
    [RelayCommand]
    private async Task LoginWithEmailAndPasswords() => await navigationService.GoToAsync(state: RoutePages.LOGIN_PAGE);

    [RelayCommand]
    private void LoginWithGoogle()
    {

    }

    [RelayCommand]
    private async Task RegisterUser() => await navigationService.GoToAsync(state: RoutePages.USER_REGISTER_PAGE);
}