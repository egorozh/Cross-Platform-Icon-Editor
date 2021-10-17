﻿namespace ViewportTwoD;

internal class CoordinateSystemLogic
{
    private readonly Viewport _viewport;

    private bool _moving;
    private Point _lastPoint;

    public CoordinateSystemLogic(Viewport viewport)
    {
        _viewport = viewport;
    }

    public Point GetGlobalPoint(Point localPoint)
        => new((localPoint.X - _viewport.DeltaX) / _viewport.Zoom,
            (localPoint.Y - _viewport.DeltaY) / _viewport.Zoom);

    public Point GetLocalPoint(Point globalPoint)
        => new(globalPoint.X * _viewport.Zoom + _viewport.DeltaX,
            globalPoint.Y * _viewport.Zoom + _viewport.DeltaY);

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
        _viewport.Zoom += e.Delta.Y * 0.5;
    }

    private void UpdatePointerCoordinates(PointerEventArgs e)
    {
        (_viewport.X, _viewport.Y) = GetGlobalPoint(e.GetPosition(_viewport.MainCanvas));
    }
}