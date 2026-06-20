using PlanShare.App.ViewModels.Pages.User.Register;

namespace PlanShare.App.Views.Pages.User.Register;

public partial class RegisterUserPage : ContentPage
{
    public RegisterUserPage()
    {
        InitializeComponent();
        BindingContext = new RegisterUserViewModel();
    }
}