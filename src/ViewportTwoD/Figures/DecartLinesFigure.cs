using System.Collections;

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

    public DecartLinesFigure()
    {
        IsShow = false;
    }

    protected internal override void Render(DrawingContext context)
    {
        if (!IsShow)
            return;

        var (centerX, centerY) = Viewport.CoordinateSystem.GetLocalPoint(new Point(0, 0));
        var width = Viewport.Bounds.Width;
        var height = Viewport.Bounds.Height;

        var k = Math.Tan(Viewport.Angle * (Math.PI / 180.0));
        var b = centerY - k * centerX;

        var success = GetLinePoints(b, k, height, width, out var p1, out var p2);

        if (success)
            context.DrawLine(new Pen(Stroke), p1, p2);

        k = Math.Tan((Viewport.Angle + 90.0) * (Math.PI / 180.0));
        b = centerY - k * centerX;

        success = GetLinePoints(b, k, height, width, out p1, out p2);

        if (success)
            context.DrawLine(new Pen(Stroke), p1, p2);
    }

    private static bool GetLinePoints(double b1, double k1, double height, double width, out Point p1, out Point p2)
    {
        p1 = new Point(-b1 / k1, 0);
        p2 = new Point((height - b1) / k1, height);
        Point p3 = new(0, b1);
        Point p4 = new(width, k1 * width + b1);

        var isFindFirstPoint = CheckPoint(p1, width, height);
        
        if (CheckPoint(p2, width, height))
        {
            if (isFindFirstPoint)
                return true;

            p1 = p2;
            isFindFirstPoint = true;
        }

        if (CheckPoint(p3, width, height))
        {
            if (isFindFirstPoint)
            {
                p2 = p3;
                return true;
            }

            p1 = p3;
            isFindFirstPoint = true;
        }

        if (CheckPoint(p4, width, height))
        {
            if (isFindFirstPoint)
            {
                p2 = p4;
                return true;
            }
        }
        
        return false;
    }

    private static bool CheckPoint(Point point, double width, double height)
        => point.X >= 0 && point.X <= width && point.Y >= 0 && point.Y <= height;
}