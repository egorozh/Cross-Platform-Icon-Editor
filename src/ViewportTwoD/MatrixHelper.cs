using static System.Math;

namespace ViewportTwoD;

/// <summary>
/// Avalonia Matrix helper methods.
/// </summary>
public static class MatrixHelper
{
    /// <summary>
    /// Creates a translation matrix using the specified offsets.
    /// </summary>
    /// <param name="offsetX">X-coordinate offset.</param>
    /// <param name="offsetY">Y-coordinate offset.</param>
    /// <returns>The created translation matrix.</returns>
    public static Matrix Translate(in double offsetX, in double offsetY)
        => new(1.0, 0.0, 0.0, 1.0, offsetX, offsetY);

    /// <summary>
    /// Prepends a translation around the center of provided matrix.
    /// </summary>
    /// <param name="matrix">The matrix to prepend translation.</param>
    /// <param name="offsetX">X-coordinate offset.</param>
    /// <param name="offsetY">Y-coordinate offset.</param>
    /// <returns>The created translation matrix.</returns>
    public static Matrix TranslatePrepend(in Matrix matrix, in double offsetX, in double offsetY)
        => Translate(offsetX, offsetY) * matrix;

    /// <summary>
    /// Creates a matrix that scales along the x-axis and y-axis.
    /// </summary>
    /// <param name="scaleX">Scaling factor that is applied along the x-axis.</param>
    /// <param name="scaleY">Scaling factor that is applied along the y-axis.</param>
    /// <returns>The created scaling matrix.</returns>
    public static Matrix Scale(in double scaleX, in double scaleY)
        => new(scaleX, 0, 0, scaleY, 0.0, 0.0);

    /// <summary>
    /// Creates a matrix that is scaling from a specified center.
    /// </summary>
    /// <param name="scaleX">Scaling factor that is applied along the x-axis.</param>
    /// <param name="scaleY">Scaling factor that is applied along the y-axis.</param>
    /// <param name="centerX">The center X-coordinate of the scaling.</param>
    /// <param name="centerY">The center Y-coordinate of the scaling.</param>
    /// <returns>The created scaling matrix.</returns>
    public static Matrix ScaleAt(in double scaleX, in double scaleY, in double centerX, in double centerY)
        => new(scaleX, 0, 0, scaleY, centerX - (scaleX * centerX), centerY - (scaleY * centerY));

    /// <summary>
    /// Prepends a scale around the center of provided matrix.
    /// </summary>
    /// <param name="matrix">The matrix to prepend scale.</param>
    /// <param name="scaleX">Scaling factor that is applied along the x-axis.</param>
    /// <param name="scaleY">Scaling factor that is applied along the y-axis.</param>
    /// <param name="centerX">The center X-coordinate of the scaling.</param>
    /// <param name="centerY">The center Y-coordinate of the scaling.</param>
    /// <returns>The created scaling matrix.</returns>
    public static Matrix ScaleAtPrepend(in Matrix matrix, in double scaleX, in double scaleY,
        double centerX, double centerY) => ScaleAt(scaleX, scaleY, centerX, centerY) * matrix;

    /// <summary>
    /// Creates a translation and scale matrix using the specified offsets and scales along the x-axis and y-axis.
    /// </summary>
    /// <param name="scaleX">Scaling factor that is applied along the x-axis.</param>
    /// <param name="scaleY">Scaling factor that is applied along the y-axis.</param>
    /// <param name="offsetX">X-coordinate offset.</param>
    /// <param name="offsetY">Y-coordinate offset.</param>
    /// <returns>The created translation and scale matrix.</returns>
    public static Matrix ScaleAndTranslate(in double scaleX, in double scaleY, in double offsetX, in double offsetY)
        => new(scaleX, 0.0, 0.0, scaleY, offsetX, offsetY);

    /// <summary>
    /// Creates a skew matrix.
    /// </summary>
    /// <param name="angleX">Angle of skew along the X-axis in radians.</param>
    /// <param name="angleY">Angle of skew along the Y-axis in radians.</param>
    /// <returns>When the method completes, contains the created skew matrix.</returns>
    public static Matrix Skew(in float angleX, in float angleY)
        => new(1.0, Tan(angleX), Tan(angleY), 1.0, 0.0, 0.0);

    /// <summary>
    /// Creates a matrix that rotates.
    /// </summary>
    /// <param name="radians">Angle of rotation in radians. Angles are measured clockwise when looking along the rotation axis.</param>
    /// <returns>The created rotation matrix.</returns>
    public static Matrix Rotation(in double radians)
    {
        var cos = Cos(radians);
        var sin = Sin(radians);
        return new Matrix(cos, sin, -sin, cos, 0, 0);
    }

    /// <summary>
    /// Creates a matrix that rotates about a specified center.
    /// </summary>
    /// <param name="angle">Angle of rotation in radians.</param>
    /// <param name="centerX">The center X-coordinate of the rotation.</param>
    /// <param name="centerY">The center Y-coordinate of the rotation.</param>
    /// <returns>The created rotation matrix.</returns>
    public static Matrix Rotation(in double angle, in double centerX, in double centerY)
        => Translate(-centerX, -centerY) * Rotation(angle) * Translate(centerX, centerY);

    /// <summary>
    /// Creates a matrix that rotates about a specified center.
    /// </summary>
    /// <param name="angle">Angle of rotation in radians.</param>
    /// <param name="center">The center of the rotation.</param>
    /// <returns>The created rotation matrix.</returns>
    public static Matrix Rotation(in double angle, in Vector center)
        => Translate(-center.X, -center.Y) * Rotation(angle) * Translate(center.X, center.Y);

    /// <summary>
    /// Transforms a point by this matrix.
    /// </summary>
    /// <param name="matrix">The matrix to use as a transformation matrix.</param>
    /// <param name="point">>The original point to apply the transformation.</param>
    /// <returns>The result of the transformation for the input point.</returns>
    public static Point TransformPoint(in Matrix matrix, in Point point)
        => new((point.X * matrix.M11) + (point.Y * matrix.M21) + matrix.M31,
            (point.X * matrix.M12) + (point.Y * matrix.M22) + matrix.M32);

    public static Matrix RotateAt(Matrix matrix, double angle, in double centerX, in double centerY)
    {
        angle %= 360.0; // Doing the modulo before converting to radians reduces total error
        matrix *= Rotation(angle * (PI / 180.0), centerX, centerY);
        return matrix;
    }
}