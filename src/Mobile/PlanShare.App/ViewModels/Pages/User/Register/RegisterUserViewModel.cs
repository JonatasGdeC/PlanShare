using CommunityToolkit.Mvvm.Input;
using PlanShare.App.Navigation;

namespace PlanShare.App.ViewModels.Pages.User.Register;

public partial class RegisterUserViewModel(INavigationService navigationService) : ViewModelBase
{
    [RelayCommand]
    private async Task LoginWithEmailAndPasswords() => await navigationService.GoToAsync(state: $"../{RoutePages.LOGIN_PAGE}");
}