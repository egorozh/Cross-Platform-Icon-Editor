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
        var matrix = MatrixHelper.Rotation(-_viewport.Angle, _viewport.DeltaX, _viewport.DeltaY);

        matrix = MatrixHelper.TranslatePrepend(matrix, -_viewport.DeltaX, -_viewport.DeltaY);

        matrix = MatrixHelper.Scale(_viewport.Resolution, _viewport.Resolution) * matrix;
        
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
        var newZoom = (decimal) _viewport.Zoom + (decimal) e.Delta.Y * 0.2M;

        if (newZoom > 0)
            _viewport.Zoom = (double) newZoom;
    }

    private void UpdatePointerCoordinates(PointerEventArgs e)
    {
        (_viewport.PointerX, _viewport.PointerY) = GetGlobalPoint(e.GetPosition(_viewport.MainCanvas));
    }
}