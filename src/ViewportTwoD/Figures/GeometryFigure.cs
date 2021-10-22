namespace ViewportTwoD;

public class GeometryFigure : Figure
{
    private readonly Path _shape;
    private IBrush? _fill;
    private PathGeometry _geometry;
    private IBrush? _stroke;
    private double _strokeThickness;
    private Pen _pen = new Pen();

    /// <summary>
    /// In Global Coords    
    /// </summary>
    public PathGeometry Geometry
    {
        get => _geometry;
        set
        {
            _geometry = value;
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
            _pen.Brush = value;
            _shape.Stroke = value;
        }
    }

    public double StrokeThickness
    {
        get => _strokeThickness;
        set
        {
            _strokeThickness = value;
            _pen.Thickness = value;
            _shape.StrokeThickness = value;
        }
    }

    public GeometryFigure()
    {
        _shape = new Path
        {
            Fill = Fill,
            Stroke = Stroke,
            StrokeThickness = StrokeThickness,
            RenderTransformOrigin = new RelativePoint(0, 0, RelativeUnit.Absolute),
            RenderTransform = new MatrixTransform()
        };
    }

    public GeometryFigure(PathGeometry geometry, IBrush fill, IBrush stroke) : this()
    {
        Geometry = geometry;
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

    protected override void Update(in Matrix transform)
    {
        ((MatrixTransform)_shape.RenderTransform).Matrix = transform;
        //_shape.RenderTransform = new MatrixTransform(transform);
    }

    protected override void Render(DrawingContext context, in Matrix transform)
    {
        using (context.PushPreTransform(transform))
            context.DrawGeometry(Fill, _pen, Geometry);
    }
}