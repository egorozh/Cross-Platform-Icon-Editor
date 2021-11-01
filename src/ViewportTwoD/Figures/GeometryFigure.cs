namespace ViewportTwoD;

public class GeometryFigure : Figure
{
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
        }
    }

    public IBrush? Fill
    {
        get => _fill;
        set
        {
            _fill = value;
        }
    }

    public IBrush? Stroke
    {
        get => _stroke;
        set
        {
            _stroke = value;
            _pen.Brush = value;
        }
    }

    public double StrokeThickness
    {
        get => _strokeThickness;
        set
        {
            _strokeThickness = value;
            _pen.Thickness = value;
        }
    }

    public GeometryFigure() { }

    public GeometryFigure(PathGeometry geometry, IBrush fill, IBrush stroke)
    {
        Geometry = geometry;
        Fill = fill;
        Stroke = stroke;
    }
    
    protected override void Render(DrawingContext context, in Matrix transform)
    {
        using (context.PushPreTransform(transform))
            context.DrawGeometry(Fill, _pen, Geometry);
    }
}