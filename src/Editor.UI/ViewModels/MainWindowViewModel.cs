using Avalonia;
using Avalonia.Media;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ViewportTwoD;

namespace Editor.UI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<Figure> Figures { get; }

    public MainWindowViewModel()
    {
        var points1 = new List<Point>()
        {
            new(30, -30),
            new(80, 20),
            new(70, 70),
            new(40, 50),
        };
        var points2 = new List<Point>()
        {
            new(180, 120),
            new(170, 170),
        };

        var data1 = new PathGeometry
        {
            Figures = new PathFigures
            {
                new()
                {
                    StartPoint = new Point(-50, -50),
                    Segments = new PathSegments {new PolyLineSegment(points1)}
                }
            }
        };

        var data2 = new PathGeometry
        {
            Figures = new PathFigures
            {
                new()
                {
                    StartPoint = new Point(130, 130),

                    Segments = new PathSegments {new PolyLineSegment(points2)}
                }
            }
        };

        Figures = new ObservableCollection<Figure>()
        {
            new GeometryFigure(data1, Brushes.Blue, Brushes.Green),
            new GeometryFigure(data2, Brushes.Red, Brushes.Violet),
            new DecartLinesFigure()
        };
    }
}