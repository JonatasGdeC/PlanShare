using CommunityToolkit.Mvvm.Input;

namespace PlanShare.App.ViewModels.Pages.OnBoarding;

public partial class OnBoardingViewModel
{
    [RelayCommand]
    private async Task LoginWithEmailAndPasswords()
    {
        await Shell.Current.GoToAsync(state: "DoLoginPage");
    }

    [RelayCommand]
    private void LoginWithGoogle()
    {

    }
}