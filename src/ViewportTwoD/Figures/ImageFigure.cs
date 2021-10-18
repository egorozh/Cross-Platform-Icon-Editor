using Avalonia.Media.Imaging;

namespace ViewportTwoD;

public class ImageFigure : Figure
{
    private readonly Image _image;

    public ImageFigure(string imagePath)
    {
        _image = new Image
        {
            Source = new Bitmap(imagePath)
        };
    }

    protected internal override void Add(Canvas canvas, Viewport viewport)
    {
        canvas.Children.Add(_image);
    }

    protected internal override void Remove(Canvas canvas)
    {
        canvas.Children.Remove(_image);
    }

    protected internal override void Update(Viewport viewport)
    {
        _image.RenderTransform = viewport.GetLocalTransform();
        _image.RenderTransformOrigin = new RelativePoint(0, 0, RelativeUnit.Absolute);
    }
}