namespace ViewportTwoD;

public abstract class Figure : AvaloniaObject
{
    #region Protected Properties

    protected Viewport Viewport { get; private set; } = null!;

    #endregion

    #region Avalonia Properties

    public static StyledProperty<Transform?> TransformProperty =
        AvaloniaProperty.Register<Figure, Transform?>(nameof(Transform));

    public static StyledProperty<bool> IsShowProperty =
        AvaloniaProperty.Register<Figure, bool>(nameof(IsShow), true);

    #endregion

    #region Public Properties

    public Transform? Transform
    {
        get => GetValue(TransformProperty);
        set => SetValue(TransformProperty, value);
    }

    public bool IsShow
    {
        get => GetValue(IsShowProperty);
        set => SetValue(IsShowProperty, value);
    }

    #endregion
    
    #region Internal Methods

    internal void Init(Viewport viewport)
    {
        Viewport = viewport;
    }

    #endregion

    #region Protected Methods

    protected internal virtual void Render(DrawingContext context)
    {
        if (!IsShow)
            return;

        var matrix = Transform?.Value ?? Matrix.Identity;

        matrix *= Viewport.CoordinateSystem.GetLocalMatrix();

        Render(context, matrix);
    }

    protected virtual void Render(DrawingContext context, in Matrix transform)
    {
    }

    #endregion
}