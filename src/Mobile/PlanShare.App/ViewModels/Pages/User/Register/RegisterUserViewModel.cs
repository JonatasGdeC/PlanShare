using CommunityToolkit.Mvvm.Input;
using PlanShare.App.Navigation;

namespace PlanShare.App.ViewModels.Pages.User.Register;

public partial class RegisterUserViewModel
{
    [RelayCommand]
    private async Task LoginWithEmailAndPasswords() => await Shell.Current.GoToAsync(state: $"../{RoutePages.LOGIN_PAGE}");
}