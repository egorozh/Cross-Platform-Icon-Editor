using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Metadata;
using Avalonia.Styling;

namespace ViewportTwoD
{
    public class Viewport : TemplatedControl, IStyleable
    {
        private const string PART_MainCanvas = "PART_MainCanvas";

        private Canvas _mainCanvas;
        private double _deltaX = 50;
        private double _deltaY = 50;
        private double _zoom = 1;

        #region Avalonia Properties

        public static StyledProperty<double> XProperty =
            AvaloniaProperty.Register<Viewport, double>(nameof(X));

        public static StyledProperty<double> YProperty =
            AvaloniaProperty.Register<Viewport, double>(nameof(Y));

        public static StyledProperty<ObservableCollection<Figure>> FiguresProperty =
            AvaloniaProperty.Register<Viewport, ObservableCollection<Figure>>(nameof(Figures));

        private bool _moving;
        private Point _lastPoint;

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

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _mainCanvas = e.NameScope.Find(PART_MainCanvas) as Canvas
                          ?? throw new Exception($"{PART_MainCanvas} not found in current Style");

            _mainCanvas.PointerMoved += MainCanvasOnPointerMoved;

            if (Figures != null)
            {
                foreach (var figure in Figures)
                {
                    figure.Add(_mainCanvas);
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
            Zoom += e.Delta.Y * 0.5;
        }


        //protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
        //{
        //    base.OnPropertyChanged(change);

        //    if (change.Property == FiguresProperty)
        //    {
        //        foreach (var figure in Figures)
        //        {
        //            ((GeometryFigure)figure).Add(_mainCanvas);
        //        }
        //    }
        //}

        private void MainCanvasOnPointerMoved(object? sender, PointerEventArgs e)
        {
            var pos = e.GetPosition(_mainCanvas);

            var transform = new TransformGroup();
            
            transform.Children.Add(new TranslateTransform(DeltaX, DeltaY));
            transform.Children.Add(new ScaleTransform(Zoom, Zoom));

            var transformValue = transform.Value;

            var (x, y) = pos.Transform(transform.Value);

            X = (pos.X - DeltaX) / Zoom;
            Y = (pos.Y - DeltaY) / Zoom;
        }

        private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            _moving = true;
            e.Pointer.Capture(_mainCanvas);
            _lastPoint = e.GetPosition(_mainCanvas);
        }

        private void OnPointerMoved(object? sender, PointerEventArgs e)
        {
            if (_moving)
            {
                var point = e.GetPosition(_mainCanvas);

                var (deltax, deltaY) = point - _lastPoint;

                DeltaX += deltax;
                DeltaY += deltaY;

                _lastPoint = point;
            }
        }

        private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            _moving = false;
            e.Pointer.Capture(null);
        }

        private void Update()
        {
            foreach (var figure in Figures)
                figure.Update(DeltaX, DeltaY, Zoom, this);
        }

        public Point GetGlobalPoint(Point localPoint)
        {
            return new Point((localPoint.X - DeltaX) / Zoom,
                (localPoint.Y - DeltaY) / Zoom);
        }

        public Point GetLocalPoint(Point globalPoint) 
            => new (globalPoint.X * Zoom + DeltaX, globalPoint.Y * Zoom + DeltaY);
    }
}