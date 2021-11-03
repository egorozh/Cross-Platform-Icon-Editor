namespace ViewportTwoD;

public class Viewport : TemplatedControl, IStyleable
{
    #region Constants

    private const string PartMainCanvas = "PART_MainCanvas";
    private const string PartRenderCanvas = "PART_RenderCanvas";

    #endregion

    #region Private Fields

    private IEnumerable<Figure>? _items = new ObservableCollection<Figure>();

    private readonly CoordinateSystemLogic _coordinateSystem;

    private double _resolution;

    #endregion

    #region Internal Fields

    internal Canvas MainCanvas = null!;
    internal CanvasEx RenderCanvas = null!;

    #endregion

    #region Avalonia Properties

    public static StyledProperty<double> PointerXProperty =
        AvaloniaProperty.Register<Viewport, double>(nameof(PointerX));

    public static StyledProperty<double> PointerYProperty =
        AvaloniaProperty.Register<Viewport, double>(nameof(PointerY));

    public static StyledProperty<double> DeltaXProperty =
        AvaloniaProperty.Register<Viewport, double>(nameof(DeltaX));

    public static StyledProperty<double> DeltaYProperty =
        AvaloniaProperty.Register<Viewport, double>(nameof(DeltaY));

    public static StyledProperty<double> AngleProperty =
        AvaloniaProperty.Register<Viewport, double>(nameof(Angle));

    public static StyledProperty<double> ZoomProperty =
        AvaloniaProperty.Register<Viewport, double>(nameof(Zoom), 1.0,
            coerce: ValidateZoomProperty);

    public static DirectProperty<Viewport, double> ResolutionProperty =
        AvaloniaProperty.RegisterDirect<Viewport, double>("Resolution",
            v => v.Resolution);

    private static double ValidateZoomProperty(IAvaloniaObject v, double arg) => arg > 0 ? arg : 0.001;

    public static readonly DirectProperty<Viewport, IEnumerable<Figure>?> FiguresProperty =
        AvaloniaProperty.RegisterDirect<Viewport, IEnumerable<Figure>?>
            (nameof(Figures), o => o.Figures, (o, v) => o.Figures = v);

    #endregion

    #region Public Properties

    public double PointerX
    {
        get => GetValue(PointerXProperty);
        set => SetValue(PointerXProperty, value);
    }

    public double PointerY
    {
        get => GetValue(PointerYProperty);
        set => SetValue(PointerYProperty, value);
    }

    [Content]
    public IEnumerable<Figure>? Figures
    {
        get => _items;
        set => SetAndRaise(FiguresProperty, ref _items, value);
    }

    public double DeltaX
    {
        get => GetValue(DeltaXProperty);
        set => SetValue(DeltaXProperty, value);
    }

    public double DeltaY
    {
        get => GetValue(DeltaYProperty);
        set => SetValue(DeltaYProperty, value);
    }

    public double Angle
    {
        get => GetValue(AngleProperty);
        set => SetValue(AngleProperty, value);
    }

    public double Zoom
    {
        get => GetValue(ZoomProperty);
        set => SetValue(ZoomProperty, value);
    }

    public double Resolution
    {
        get => _resolution;
        private set => SetAndRaise(ResolutionProperty, ref _resolution, value);
    }

    #endregion

    #region IStyleable

    Type IStyleable.StyleKey => typeof(Viewport);

    #endregion

    #region Constructor

    public Viewport()
    {
        _coordinateSystem = new CoordinateSystemLogic(this);
    }

    #endregion

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        MainCanvas = e.NameScope.Find(PartMainCanvas) as Canvas
                     ?? throw new Exception($"{PartMainCanvas} not found in current Style");

        RenderCanvas = e.NameScope.Find(PartRenderCanvas) as CanvasEx
                       ?? throw new Exception($"{PartRenderCanvas} not found in current Style");

        RenderCanvas.Init(this);

        if (Figures != null)
        {
            foreach (var figure in Figures)
                figure.Init(this);
        }

        RenderCanvas.InvalidateVisual();

        this.PointerPressed += OnPointerPressed;
        this.PointerReleased += OnPointerReleased;
        this.PointerMoved += OnPointerMoved;

        this.PointerWheelChanged += OnPointerWheelChanged;
    }

    protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change) 
    {
        base.OnPropertyChanged(change);

        var prop = change.Property;

        if (prop == ZoomProperty)
        {
            Resolution = (double) (1M / (decimal) Zoom);
            UpdateFigures();
        }
        else if (prop == BoundsProperty || prop == DeltaXProperty ||
                 prop == DeltaYProperty || prop == ResolutionProperty ||
                 prop == AngleProperty)
        {
            UpdateFigures();
        }
    }

    private void OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        _coordinateSystem.PointerWheelChanged(e);
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        _coordinateSystem.PointerPressed(e);
    }

    private void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        _coordinateSystem.PointerMoved(e);
    }

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        _coordinateSystem.PointerReleased(e);
    }

    private void UpdateFigures()
    {
        RenderCanvas?.InvalidateVisual();
    }

    public Point GetGlobalPoint(in Point localPoint)
        => _coordinateSystem.GetGlobalPoint(localPoint);

    public Point GetLocalPoint(in Point globalPoint)
        => _coordinateSystem.GetLocalPoint(globalPoint);

    public Matrix GetLocalMatrix()
        => _coordinateSystem.GetLocalMatrix();
}