namespace PlanShare.App.Views.Components.Input;

public partial class InputPasswordComponent : ContentView
{
    public string Title { get => (string)GetValue(property: TitleProperty); set => SetValue(property: TitleProperty, value: value); }
    private static readonly BindableProperty TitleProperty = BindableProperty.Create(propertyName: nameof(Title), returnType: typeof(string), declaringType: typeof(InputPasswordComponent), defaultValue: "Senha");
    
    public InputPasswordComponent()
    {
        InitializeComponent();
    }

    private void ShowHidePassword(object sender, EventArgs e)
    {
        if (EntryPassword.IsPassword)
        {
            EntryPassword.IsPassword = false;
            ImageEye.Source = "icon_eye.png";
        }
        else
        {
            EntryPassword.IsPassword = true;
            ImageEye.Source = "icon_eye_hidden.png";
        }
    }
}