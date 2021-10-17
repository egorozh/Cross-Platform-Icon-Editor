namespace ViewportTwoD;

public abstract class Figure
{
    protected internal abstract void Add(Canvas canvas);

    protected internal abstract void Remove(Canvas canvas);

    protected internal abstract void Update(double deltaX, double deltaY, double zoom, Viewport viewport);
}