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