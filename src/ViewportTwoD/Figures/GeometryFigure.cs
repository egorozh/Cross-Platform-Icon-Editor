namespace ViewportTwoD;

public class GeometryFigure : Figure
{
    private readonly Path _shape;
    private IBrush? _fill;
    private PathGeometry _path;
    private IBrush? _stroke;

    /// <summary>
    /// In Global Coords
    /// </summary>
    public PathGeometry Path
    {
        get => _path;
        set
        {
            _path = value;
            _shape.Data = value;
        }
    }

    public IBrush? Fill
    {
        get => _fill;
        set
        {
            _fill = value;
            _shape.Fill = value;
        } 
    }

    public IBrush? Stroke
    {
        get => _stroke;
        set
        {
            _stroke = value;
            _shape.Stroke = value;
        }
    }

    public GeometryFigure()
    {
        _shape = new Path()
        {
            Fill = Fill,
            Stroke = Stroke
        };
    }

    public GeometryFigure(PathGeometry path, IBrush fill, IBrush stroke) : this()
    {
        Path = path;
        Fill = fill;
        Stroke = stroke;
    }

    protected internal override void Add(Canvas canvas)
    {
        canvas.Children.Add(_shape);
    }

    protected internal override void Remove(Canvas canvas)
    {
        canvas.Children.Remove(_shape);
    }

    protected internal override void Update(Viewport viewport)
    {
        _shape.Data = Path;
        
        _shape.RenderTransform = viewport.GetLocalTransform();
        _shape.RenderTransformOrigin = new RelativePoint(0, 0, RelativeUnit.Absolute);
    }
}