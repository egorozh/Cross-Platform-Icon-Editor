namespace ViewportTwoD;

public class DecartLinesFigure : Figure
{
    private readonly Line _lineX;
    private readonly Line _lineY;

    public DecartLinesFigure()
    {
        _lineX = new Line()
        {
            Fill = Brushes.Red,
            Stroke = Brushes.Red,
            StrokeThickness = 1
        };

        _lineY = new Line()
        {
            Fill = Brushes.Red,
            Stroke = Brushes.Red,
            StrokeThickness = 1
        };
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

    protected internal override void Update(double deltaX, double deltaY, double zoom, Viewport viewport)
    {
        var xPoint1 = new Point(-10000, 0);
        var xPoint2 = new Point(10000, 0);
        var yPoint1 = new Point(0, -10000);
        var yPoint2 = new Point(0, 10000);

        _lineX.StartPoint = viewport.GetLocalPoint(xPoint1);
        _lineX.EndPoint = viewport.GetLocalPoint(xPoint2);

        _lineY.StartPoint = viewport.GetLocalPoint(yPoint1);
        _lineY.EndPoint = viewport.GetLocalPoint(yPoint2);
    }
}