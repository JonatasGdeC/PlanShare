namespace PlanShare.App.Views.Components.Input;

public partial class InputComponent : ContentView
{
    public string Title { get => (string)GetValue(property: TitleProperty); set => SetValue(property: TitleProperty, value: value); }
    private static readonly BindableProperty TitleProperty = BindableProperty.Create(propertyName: nameof(Title), returnType: typeof(string), declaringType: typeof(InputComponent), defaultValue: string.Empty);
    
    public string Placeholder { get => (string)GetValue(property: PlaceholderProperty); set => SetValue(property: PlaceholderProperty, value: value); }
    private static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(propertyName: nameof(Placeholder), returnType: typeof(string), declaringType: typeof(InputComponent), defaultValue: string.Empty);
    
    public Keyboard Keyboard { get => (Keyboard)GetValue(property: KeyboardProperty); set => SetValue(property: KeyboardProperty, value: value); }
    private static readonly BindableProperty KeyboardProperty = BindableProperty.Create(propertyName: nameof(Keyboard), returnType: typeof(Keyboard), declaringType: typeof(InputComponent), defaultValue: Keyboard.Default);
    
    public InputComponent()
	{
		InitializeComponent();
	}
}