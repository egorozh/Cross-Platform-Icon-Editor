namespace ViewportTwoD;

public abstract class Figure : AvaloniaObject
{
    protected Viewport Viewport { get; private set; } = null!;

    public static StyledProperty<Transform?> TransformProperty =
        AvaloniaProperty.Register<Figure, Transform?>(nameof(Transform));

    public Transform? Transform
    {
        get => GetValue(TransformProperty);
        set => SetValue(TransformProperty, value);
    }

    internal void Init(Viewport viewport)
    {
        Viewport = viewport;
    }

    protected internal abstract void Add(Canvas canvas);

    protected internal abstract void Remove(Canvas canvas);

    protected internal virtual void Update()
    {
        var matrix = Transform?.Value ?? Matrix.Identity;

        matrix *= Viewport.GetLocalMatrix();

        Update(matrix);
    }

    protected virtual void Update(in Matrix transform)
    {
    }

    protected internal virtual void Render(DrawingContext context)
    {
        var matrix = Transform?.Value ?? Matrix.Identity;

        matrix *= Viewport.GetLocalMatrix();

        Render(context,matrix);
    }
        
    protected virtual void Render(DrawingContext context, in Matrix transform)
    {
    }
}