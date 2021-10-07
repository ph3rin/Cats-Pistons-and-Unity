using UnityEngine;

namespace CatProcessingUnit
{
    public static class StickablityChecker
    {
        public static bool AreStickable(WorkshopTile tileA, WorkshopTile tileB)
        {
            var distance = ManhattanMagnitude(tileA.Position - tileB.Position);
            Debug.Assert(distance == 1);
            
            
            
            if (tileA is PistonArmTile armA &&
                tileB is PistonArmTile armB &&
                armA.Orientation == -armB.Orientation)
            {
                return false;
            }

            return true;
        }

        private static int ManhattanMagnitude(Vector2Int vec)
        {
            return Mathf.Abs(vec.x) + Mathf.Abs(vec.y);
        }

        private static bool IsPistonOrArm(WorkshopTile tile)
        {
            return tile is PistonTile piston || tile is PistonArmTile;
        } 
        
    }
}