namespace ViewportTwoD;

public abstract class Figure : AvaloniaObject
{
    public static StyledProperty<Transform?> TransformProperty =
        AvaloniaProperty.Register<Figure, Transform?>(nameof(Transform));

    public Transform? Transform
    {
        get => GetValue(TransformProperty);
        set => SetValue(TransformProperty, value);
    }

    protected internal abstract void Add(Canvas canvas, Viewport viewport);

    protected internal abstract void Remove(Canvas canvas);

    protected internal virtual void Update(Viewport viewport)
    {
        var transform = new TransformGroup();

        if (Transform != null)
            transform.Children.Add(Transform);

        transform.Children.Add(viewport.GetLocalTransform());

        Update(viewport, transform);
    }

    protected virtual void Update(Viewport viewport, Transform transform)
    {
    }
}