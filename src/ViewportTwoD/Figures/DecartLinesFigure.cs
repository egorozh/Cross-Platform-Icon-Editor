using Avalonia.Data;

namespace ViewportTwoD;

public class DecartLinesFigure : Figure
{
    private readonly Line _lineX;
    private readonly Line _lineY;

    public static StyledProperty<IBrush?> StrokeProperty =
        AvaloniaProperty.Register<DecartLinesFigure, IBrush?>(nameof(Stroke), Brushes.Blue);

    public IBrush? Stroke
    {
        get => GetValue(StrokeProperty);
        set => SetValue(StrokeProperty, value);
    }

    public DecartLinesFigure()
    {
        _lineX = new Line
        {
            StrokeThickness = 1
        };

        _lineX.Bind(Shape.StrokeProperty, new Binding()
        {
            Source = this,
            Path = "Stroke"
        });

        _lineY = new Line
        {
            StrokeThickness = 1
        };

        _lineY.Bind(Shape.StrokeProperty, new Binding()
        {
            Source = this,
            Path = "Stroke"
        });
    }

    protected internal override void Add(Canvas canvas)
    {
        canvas.Children.Add(_lineX);
        canvas.Children.Add(_lineY);
    }

    protected internal override void Remove(Canvas canvas)
    {
        canvas.Children.Remove(_lineX);
        canvas.Children.Remove(_lineY);
    }

    protected internal override void Update()
    {
        var (centerX, centerY) = Viewport.GetLocalPoint(new Point(0, 0));

        var width = Viewport.Bounds.Width;
        var height = Viewport.Bounds.Height;

        SetPoints(_lineY, centerX > 0 && centerX < width,
            new Point(centerX, 0), new Point(centerX, height));

        SetPoints(_lineX, centerY > 0 && centerY < height,
            new Point(0, centerY), new Point(width, centerY));
    }

    private static void SetPoints(Line line, bool condition, in Point start, in Point end)
    {
        line.StartPoint = condition ? start : new Point();
        line.EndPoint = condition ? end : new Point();
    }
}