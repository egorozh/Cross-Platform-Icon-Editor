using Avalonia;
using Avalonia.Xaml.Interactivity;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ViewportTwoD;

namespace Editor.UI;

internal class MixFiguresBehavior : Behavior<Viewport>
{
    private IEnumerable<Figure> _items;

    public static readonly DirectProperty<MixFiguresBehavior, IEnumerable<Figure>?> FiguresProperty =
        AvaloniaProperty.RegisterDirect<MixFiguresBehavior, IEnumerable<Figure>?>
            (nameof(Figures), o => o.Figures, (o, v) => o.Figures = v);

    public IEnumerable<Figure>? Figures
    {
        get => _items;
        set => SetAndRaise(FiguresProperty, ref _items, value);
    }

    protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == FiguresProperty)
        {
            if (change.OldValue.HasValue && change.OldValue.Value is INotifyCollectionChanged oldVmFigures)
            {
                oldVmFigures.CollectionChanged -= VmFiguresOnCollectionChanged;
            }

            if (change.NewValue.HasValue && change.NewValue.Value is ObservableCollection<Figure> vmFigures)
            {
                AddItems(vmFigures);
                vmFigures.CollectionChanged += VmFiguresOnCollectionChanged;
            }
        }
    }

    private void VmFiguresOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            AddItems(e.NewItems);
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            RemoveItems(e.OldItems);
        }
        else if (e.Action == NotifyCollectionChangedAction.Reset)
        {
            RemoveItems(e.OldItems);
        }
    }

    private void AddItems(IEnumerable vmFigures)
    {
        if (AssociatedObject is {Figures: IList xamlFigures})
        {
            foreach (var figure in vmFigures)
                xamlFigures.Add(figure);
        }
    }

    private void RemoveItems(IEnumerable vmFigures)
    {
        if (AssociatedObject is {Figures: IList xamlFigures})
        {
            foreach (var figure in vmFigures)
                xamlFigures.Remove(figure);
        }
    }
}