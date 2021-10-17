using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace ViewportTwoD;

public class GeometryFigure : Figure
{
    private readonly global::Avalonia.Controls.Shapes.Path _shape;

    /// <summary>
    /// In Global Coords
    /// </summary>
    public PathGeometry Path { get; }

    public GeometryFigure(PathGeometry path, IBrush fill, IBrush stroke)
    {
        Path = path;

        _shape = new global::Avalonia.Controls.Shapes.Path()
        {
            Fill = fill,
            Stroke = stroke
        };
    }

    //internal override void Update()
    //{
    //    _shape.Data = Path;
    //}

    internal override void Add(Canvas canvas)
    {
        canvas.Children.Add(_shape);
    }

    internal override void Remove(Canvas canvas)
    {
        canvas.Children.Remove(_shape);
    }

    internal override void Update(double deltaX, double deltaY, double zoom, Viewport viewport)
    {
        _shape.Data = Path;

        var transform = new TransformGroup();

        transform.Children.Add(new ScaleTransform(zoom, zoom));
        transform.Children.Add(new TranslateTransform(deltaX,deltaY));
     
        _shape.RenderTransform = transform;
        _shape.RenderTransformOrigin = new RelativePoint(0, 0, RelativeUnit.Absolute);
    }
}