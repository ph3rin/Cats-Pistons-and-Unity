using CatProcessingUnit.TileDataNS;
using UnityEngine;

namespace CatProcessingUnit
{
    public static class WorkshopDataPistonUtils
    {
        public static void TogglePistonStickiness(this WorkshopData data, PistonTileData piston)
        {
            piston = data.FindCounterpart(piston);
            if (piston.Sticky)
            {
                data.SetPistonStickiness(piston, false);
                data.StickyPiston = null;
            }
            else
            {
                Debug.Assert(data.StickyPiston != piston);
                data.SetPistonStickiness(data.StickyPiston, false);
                data.SetPistonStickiness(piston, true);
                data.StickyPiston = piston;
            }
        }

        public static void SetPistonStickiness(this WorkshopData data, PistonTileData piston, bool val)
        {
            if (piston == null) return;
            piston = data.FindCounterpart(piston);
            piston.Sticky = val;
            if (piston.Extended)
            {
                var arm = data.GetPistonArm(piston);
                arm.Sticky = val;
            }
        }

        public static PistonArmTileData GetPistonArm(this WorkshopData data, PistonTileData piston)
        {
            piston = data.FindCounterpart(piston);
            Debug.Assert(piston.Extended);
            var result = data.GetTileAt(piston.Position + piston.Direction) as PistonArmTileData;
            Debug.Assert(result != null, "result != null");
            return result;
        }
    }
}