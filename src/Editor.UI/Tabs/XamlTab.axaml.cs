using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Editor.UI.Tabs;

public class XamlTab : UserControl
{
    public XamlTab()
    {
        AvaloniaXamlLoader.Load(this);
    }
}