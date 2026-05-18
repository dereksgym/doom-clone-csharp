namespace DoomClone.Utilities;

public struct Ray
{
    public Vector3 Origin { get; set; }
    public Vector3 Direction { get; set; }

    public Ray(Vector3 origin, Vector3 direction)
    {
        Origin = origin;
        Direction = direction.Normalized;
    }

    public Vector3 GetPoint(float distance) => Origin + Direction * distance;
}
