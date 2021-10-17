using Avalonia.Controls;

namespace ViewportTwoD;

public abstract class Figure
{
    internal abstract void Add(Canvas canvas);

    internal abstract void Remove(Canvas canvas);

    internal abstract void Update(double deltaX, double deltaY, double zoom, Viewport viewport);
}