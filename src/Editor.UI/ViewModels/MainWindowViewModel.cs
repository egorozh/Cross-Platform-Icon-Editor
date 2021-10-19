using System;
using Avalonia.Media;
using System.Collections.ObjectModel;
using ViewportTwoD;

namespace Editor.UI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<Figure> Figures { get; }

    public MainWindowViewModel()
    {
        const string data1 =
            "M17.65,6.35C16.2,4.9 14.21,4 12,4A8,8 0 0,0 4,12A8,8 0 0,0 12,20C15.73,20 18.84,17.45 19.73,14H17.65C16.83,16.33 14.61,18 12,18A6,6 0 0,1 6,12A6,6 0 0,1 12,6C13.66,6 15.14,6.69 16.22,7.78L13,11H20V4L17.65,6.35Z";
        const string data2 =
            "M9.5,3A6.5,6.5 0 0,1 16,9.5C16,11.11 15.41,12.59 14.44,13.73L14.71,14H15.5L20.5,19L19,20.5L14,15.5V14.71L13.73,14.44C12.59,15.41 11.11,16 9.5,16A6.5,6.5 0 0,1 3,9.5A6.5,6.5 0 0,1 9.5,3M9.5,5C7,5 5,7 5,9.5C5,12 7,14 9.5,14C12,14 14,12 14,9.5C14,7 12,5 9.5,5Z";

        var figures = new ObservableCollection<Figure>
        {
            new GeometryFigure(PathGeometry.Parse(data1), Brushes.Blue, Brushes.Green),
            new GeometryFigure(PathGeometry.Parse(data2), Brushes.Red, Brushes.Violet)
            {
                StrokeThickness = 1,
                Transform = new TranslateTransform(100, 100)
            },
            new DecartLinesFigure()
        };

        var random = new Random(458);

        for (var i = 0; i < 100; i++) 
            figures.Add(CreateRandomFigure(random, data1));

        Figures = figures;
    }

    private static GeometryFigure CreateRandomFigure(Random random, string data)
    {
        var fillColor = Color.FromUInt32((uint) random
            .Next(unchecked((int) 0xff000000),int.MaxValue));
        var strokeColor = Color.FromUInt32((uint)random
            .Next(unchecked((int) 0xff000000),int.MaxValue));
        
        return new GeometryFigure(PathGeometry.Parse(data),
            new SolidColorBrush(fillColor), new SolidColorBrush(strokeColor))
        {
            StrokeThickness = random.Next(100) % 2 == 0 ? 1 : 0,
            Transform = new TranslateTransform(random.Next(1000),
                random.Next(1000))
        };
    }
}