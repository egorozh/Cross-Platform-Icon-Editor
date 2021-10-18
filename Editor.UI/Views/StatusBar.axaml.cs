using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Editor.UI.Views;

public class StatusBar : UserControl
{
    public StatusBar()
    {
        AvaloniaXamlLoader.Load(this);
    }
}