namespace DoomClone.Utilities;

public static class MathHelper
{
    public const float PI = MathF.PI;
    public const float TAU = PI * 2;
    public const float DEG_TO_RAD = PI / 180f;
    public const float RAD_TO_DEG = 180f / PI;

    public static float ToRadians(float degrees) => degrees * DEG_TO_RAD;
    public static float ToDegrees(float radians) => radians * RAD_TO_DEG;

    public static float Clamp(float value, float min, float max) => 
        value < min ? min : value > max ? max : value;

    public static float Lerp(float a, float b, float t) => a + (b - a) * Clamp(t, 0, 1);

    public static float Distance2D(float x1, float y1, float x2, float y2) =>
        MathF.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));

    public static bool LineIntersectsCircle(Vector3 lineStart, Vector3 lineEnd, Vector3 circleCenter, float radius)
    {
        var d = lineEnd - lineStart;
        var f = lineStart - circleCenter;

        float a = Vector3.Dot(d, d);
        float b = 2 * Vector3.Dot(f, d);
        float c = Vector3.Dot(f, f) - radius * radius;

        float discriminant = b * b - 4 * a * c;
        return discriminant >= 0;
    }
}
