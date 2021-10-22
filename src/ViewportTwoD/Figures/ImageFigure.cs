using Avalonia.Media.Imaging;

namespace ViewportTwoD;

public class ImageFigure : Figure
{
    private readonly Image _image;

    public ImageFigure(string imagePath)
    {
        _image = new Image
        {
            Source = new Bitmap(imagePath),
            RenderTransformOrigin = new RelativePoint(0, 0, RelativeUnit.Absolute),
            RenderTransform = new MatrixTransform()
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

    protected override void Update(in Matrix transform)
    {
        ((MatrixTransform)_image.RenderTransform).Matrix = transform;
    }
}