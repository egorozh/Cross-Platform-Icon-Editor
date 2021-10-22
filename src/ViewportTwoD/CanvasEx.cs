namespace ViewportTwoD;

public class CanvasEx : Control
{
    private Viewport _viewport = null!;

    public void Init(Viewport viewport)
    {
        _viewport = viewport;
    }

    public override void Render(DrawingContext context)
    {
        if (_viewport.Figures == null)
            return;

        foreach (var figure in _viewport.Figures)
        {
            figure.Render(context);
        }
    }
}