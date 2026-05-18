using System;

namespace DoomClone.Utilities;

public struct Vector3
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    public Vector3(float x = 0, float y = 0, float z = 0)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public float Length => MathF.Sqrt(X * X + Y * Y + Z * Z);
    public float LengthSquared => X * X + Y * Y + Z * Z;

    public Vector3 Normalized
    {
        get
        {
            float length = Length;
            if (length == 0) return new Vector3(0, 0, 0);
            return new Vector3(X / length, Y / length, Z / length);
        }
    }

    public static Vector3 operator +(Vector3 a, Vector3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Vector3 operator -(Vector3 a, Vector3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static Vector3 operator *(Vector3 a, float scalar) => new(a.X * scalar, a.Y * scalar, a.Z * scalar);
    public static Vector3 operator *(float scalar, Vector3 a) => a * scalar;
    public static Vector3 operator /(Vector3 a, float scalar) => new(a.X / scalar, a.Y / scalar, a.Z / scalar);
    public static float operator *(Vector3 a, Vector3 b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;  // Dot product

    public static Vector3 Cross(Vector3 a, Vector3 b) => new(
        a.Y * b.Z - a.Z * b.Y,
        a.Z * b.X - a.X * b.Z,
        a.X * b.Y - a.Y * b.X
    );

    public static float Distance(Vector3 a, Vector3 b) => (a - b).Length;
    public static float DistanceSquared(Vector3 a, Vector3 b) => (a - b).LengthSquared;

    public override string ToString() => $"({X:F2}, {Y:F2}, {Z:F2})";
}
