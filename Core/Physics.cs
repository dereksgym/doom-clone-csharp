namespace DoomClone.Core;

using DoomClone.Utilities;

public class Physics
{
    public static bool IsWall(World world, float x, float y, float radius = Constants.PLAYER_RADIUS)
    {
        return world.IsWall(x, y, radius);
    }

    public static bool TryMove(World world, Vector3 currentPos, Vector3 desiredPos, float radius, out Vector3 finalPos)
    {
        finalPos = currentPos;

        // Check if destination is clear
        if (!IsWall(world, desiredPos.X, desiredPos.Z, radius))
        {
            finalPos = desiredPos;
            return true;
        }

        // Try sliding along X axis
        if (!IsWall(world, desiredPos.X, currentPos.Z, radius))
        {
            finalPos = new Vector3(desiredPos.X, desiredPos.Y, currentPos.Z);
            return true;
        }

        // Try sliding along Z axis
        if (!IsWall(world, currentPos.X, desiredPos.Z, radius))
        {
            finalPos = new Vector3(currentPos.X, desiredPos.Y, desiredPos.Z);
            return true;
        }

        return false;
    }
}
