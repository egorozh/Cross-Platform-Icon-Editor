namespace ViewportTwoD;

public class DecartLinesFigure : Figure
{
    public static StyledProperty<IBrush?> StrokeProperty =
        AvaloniaProperty.Register<DecartLinesFigure, IBrush?>(nameof(Stroke), Brushes.Blue);

    public IBrush? Stroke
    {
        get => GetValue(StrokeProperty);
        set => SetValue(StrokeProperty, value);
    }


    protected internal override void Render(DrawingContext context)
    {
        //TODO Заменить на нормальный расчет точек линий
        var deltaX = Viewport.DeltaX;
        var deltaY = Viewport.DeltaY;

        using (context.PushPreTransform(MatrixHelper.Rotation(Viewport.Angle * (Math.PI / 180.0), Viewport.DeltaX, Viewport.DeltaY)))
        {
            context.DrawLine(new Pen(Stroke), new Point(-10000, deltaY), new Point(10000, deltaY));
            context.DrawLine(new Pen(Stroke), new Point(deltaX, -10000), new Point(deltaX, 10000));
        }

        //var (centerX, centerY) = Viewport.GetLocalPoint(new Point(0, 0));

        //var width = Viewport.Bounds.Width;
        //var height = Viewport.Bounds.Height;

        //if (centerX > 0 && centerX < width)
        //    context.DrawLine(new Pen(Stroke), new Point(centerX, 0), new Point(centerX, height));

        //if (centerY > 0 && centerY < height)
        //    context.DrawLine(new Pen(Stroke), new Point(0, centerY), new Point(width, centerY));
    }
}