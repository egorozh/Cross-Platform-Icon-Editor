namespace ViewportTwoD;

public class DecartLinesFigure : Figure
{
    private readonly Line _lineX;
    private readonly Line _lineY;

    public DecartLinesFigure()
    {
        _lineX = new Line
        {
            Stroke = Brushes.Blue,
            StrokeThickness = 1
        };

        _lineY = new Line
        {
            Stroke = Brushes.Blue,
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

    protected internal override void Update(Viewport viewport)
    {
        var (centerX, centerY) = viewport.GetLocalPoint(new Point(0, 0));

        var width = viewport.Bounds.Width;
        var height = viewport.Bounds.Height;

        if (centerX > 0 && centerX < width)
        {
            _lineY.StartPoint = new Point(centerX, 0);
            _lineY.EndPoint = new Point(centerX, height);
        }
        else
        {
            _lineY.StartPoint = new Point();
            _lineY.EndPoint = new Point();
        }

        if (centerY > 0 && centerY < height)
        {
            _lineX.StartPoint = new Point(0, centerY);
            _lineX.EndPoint = new Point(width, centerY);
        }
        else
        {
            _lineX.StartPoint = new Point();
            _lineX.EndPoint = new Point();
        }

        //var xPoint1 = new Point(-10000, 0);
        //var xPoint2 = new Point(10000, 0);
        //var yPoint1 = new Point(0, -10000);
        //var yPoint2 = new Point(0, 10000);

        //_lineX.StartPoint = viewport.GetLocalPoint(xPoint1);
        //_lineX.EndPoint = viewport.GetLocalPoint(xPoint2);

        //_lineY.StartPoint = viewport.GetLocalPoint(yPoint1);
        //_lineY.EndPoint = viewport.GetLocalPoint(yPoint2);
    }
}