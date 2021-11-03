namespace ViewportTwoD;

internal class CoordinateSystemLogic
{
    private readonly Viewport _viewport;

    private bool _moving;
    private Point _lastPoint;

    public CoordinateSystemLogic(Viewport viewport)
    {
        _viewport = viewport;
    }

    public Point GetGlobalPoint(in Point localPoint)
        => MatrixHelper.TransformPoint(GetGlobalMatrix(), localPoint);

    public Point GetLocalPoint(in Point globalPoint)
        => MatrixHelper.TransformPoint(GetLocalMatrix(), globalPoint);

    public Matrix GetLocalMatrix()
    {
        var matrix = MatrixHelper.ScaleAndTranslate(_viewport.Zoom, _viewport.Zoom,
            _viewport.DeltaX, _viewport.DeltaY);

        matrix = MatrixHelper.RotateAt(matrix, _viewport.Angle, _viewport.DeltaX, _viewport.DeltaY);

        return matrix;
    }

    public Matrix GetGlobalMatrix()
    {
        var matrix = MatrixHelper.Rotation(-_viewport.Angle * (Math.PI / 180.0), _viewport.DeltaX, _viewport.DeltaY);

        matrix *= MatrixHelper.Translate(-_viewport.DeltaX, -_viewport.DeltaY);

        matrix *= MatrixHelper.Scale(_viewport.Resolution, _viewport.Resolution);

        return matrix;
    }

    public void PointerPressed(PointerPressedEventArgs e)
    {
        _moving = true;
        e.Pointer.Capture(_viewport.MainCanvas);
        _lastPoint = e.GetPosition(_viewport.MainCanvas);
    }

    public void PointerMoved(PointerEventArgs e)
    {
        UpdatePointerCoordinates(e);

        if (!_moving)
            return;

        var point = e.GetPosition(_viewport.MainCanvas);

        var (deltax, deltaY) = point - _lastPoint;

        _viewport.DeltaX += deltax;
        _viewport.DeltaY += deltaY;

        _lastPoint = point;
    }

    public void PointerReleased(PointerReleasedEventArgs e)
    {
        _moving = false;
        e.Pointer.Capture(null);
    }

    public void PointerWheelChanged(PointerWheelEventArgs e)
    {
        var cursorPos = e.GetPosition(_viewport.MainCanvas);

        var globalPos = GetGlobalPoint(cursorPos);
        (_viewport.PointerX, _viewport.PointerY) = globalPos;

        Scalling(e.Delta.Y, cursorPos, globalPos);
    }

    private void UpdatePointerCoordinates(PointerEventArgs e)
    {
        (_viewport.PointerX, _viewport.PointerY) = GetGlobalPoint(e.GetPosition(_viewport.MainCanvas));
    }

    private void Scalling(double delta, Point localPos, Point globalPoint)
    {
        if (Math.Sign(delta) < 0)
        {
            if (_viewport.Zoom <= 0.2)
                return;

            _viewport.Zoom -= GetSubDelta(_viewport.Zoom);
        }
        else
        {
            if (_viewport.Zoom >= 140)
                return;

            _viewport.Zoom += GetAddDelta(_viewport.Zoom);
        }
        
        var newZoom = (decimal) _viewport.Zoom + (decimal) delta * 0.2M;

        if (newZoom > 0)
            _viewport.Zoom = (double) newZoom;

        var (newLocalX, newLocalY) = GetLocalPoint(globalPoint);
        _viewport.DeltaX -= newLocalX - localPos.X;
        _viewport.DeltaY -= newLocalY - localPos.Y;
    }

    private double GetAddDelta(double oldZoom)
    {
        return (double) 0.2M;
    }

    private double GetSubDelta(double oldZoom)
    {
        return (double) 0.2M;
    }
}