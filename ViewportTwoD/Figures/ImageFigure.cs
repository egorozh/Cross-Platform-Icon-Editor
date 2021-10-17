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

    protected internal override void Add(Canvas canvas)
    {
        canvas.Children.Add(_image);
    }

    protected internal override void Remove(Canvas canvas)
    {
        canvas.Children.Remove(_image);
    }

    protected internal override void Update(double deltaX, double deltaY, double zoom, Viewport viewport)
    {
        var transform = new TransformGroup();

        transform.Children.Add(new ScaleTransform(zoom, zoom));
        transform.Children.Add(new TranslateTransform(deltaX, deltaY));

        _image.RenderTransform = transform;
        _image.RenderTransformOrigin = new RelativePoint(0, 0, RelativeUnit.Absolute);
    }
}