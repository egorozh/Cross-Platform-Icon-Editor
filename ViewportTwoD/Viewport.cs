namespace ViewportTwoD
{
    public class Viewport : TemplatedControl, IStyleable
    {
        #region Constants

        private const string PartMainCanvas = "PART_MainCanvas";

        #endregion

        #region Private Fields

        private readonly CoordinateSystemLogic _coordinateSystem;

        private double _deltaX = 50;
        private double _deltaY = 50;
        private double _zoom = 1;

        #endregion

        #region Internal Fields

        internal Canvas MainCanvas = null!;

        #endregion

        #region Avalonia Properties

        public static StyledProperty<double> XProperty =
            AvaloniaProperty.Register<Viewport, double>(nameof(X));

        public static StyledProperty<double> YProperty =
            AvaloniaProperty.Register<Viewport, double>(nameof(Y));

        public static StyledProperty<ObservableCollection<Figure>> FiguresProperty =
            AvaloniaProperty.Register<Viewport, ObservableCollection<Figure>>(nameof(Figures));

        #endregion

        #region Public Properties

        public double X
        {
            get => GetValue(XProperty);
            set => SetValue(XProperty, value);
        }

        public double Y
        {
            get => GetValue(YProperty);
            set => SetValue(YProperty, value);
        }

        [Content]
        public ObservableCollection<Figure> Figures
        {
            get => GetValue(FiguresProperty);
            set => SetValue(FiguresProperty, value);
        }

        public double DeltaX
        {
            get => _deltaX;
            set
            {
                _deltaX = value;
                Update();
            }
        }

        public double DeltaY
        {
            get => _deltaY;
            set
            {
                _deltaY = value;
                Update();
            }
        }

        public double Zoom
        {
            get => _zoom;
            set
            {
                if (value <= 0)
                    return;

                _zoom = value;
                Update();
            }
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

            if (Figures != null)
            {
                foreach (var figure in Figures)
                {
                    figure.Add(MainCanvas);
                    figure.Update(DeltaX, DeltaY, Zoom, this);
                }
            }

            this.PointerPressed += OnPointerPressed;
            this.PointerReleased += OnPointerReleased;
            this.PointerMoved += OnPointerMoved;

            this.PointerWheelChanged += OnPointerWheelChanged;
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

        private void Update()
        {
            foreach (var figure in Figures)
                figure.Update(DeltaX, DeltaY, Zoom, this);
        }

        public Point GetGlobalPoint(Point localPoint)
            => _coordinateSystem.GetGlobalPoint(localPoint);

        public Point GetLocalPoint(Point globalPoint)
            => _coordinateSystem.GetLocalPoint(globalPoint);
    }
}