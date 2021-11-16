using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Editor.UI.Tabs;

public class MvvmTab : UserControl
{
    public MvvmTab()
    {
        AvaloniaXamlLoader.Load(this);
    }
}