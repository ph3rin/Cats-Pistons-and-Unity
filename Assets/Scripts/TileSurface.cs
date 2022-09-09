using System.Linq;
using UnityEngine;

namespace CatProcessingUnit
{
    public partial class TileSurface
    {
        public SurfaceFlags[] Flags { get; }

        public TileSurface(SurfaceFlags[] flags)
        {
            Debug.Assert(flags.Length == 8);
            Flags = flags;
        }

        public TileSurface Rotate(int times90Degrees)
        {
            Debug.Assert(times90Degrees >= 0 && times90Degrees <= 3);
            var offset = times90Degrees * 2;
            var newFlags = new SurfaceFlags[8];
            for (var i = 0; i < 8; ++i)
            {
                newFlags[i] = Flags[(i + 8 - offset) % 8];
            }

            return new TileSurface(newFlags);
        }

        public TileSurface RotateTo(Vector2Int direction)
        {
            return Rotate(DirectionUtils.DirectionToIndex(direction));
        }

        public SurfaceFlags GetFlags(Vector2Int dir, int index)
        {
            Debug.Assert(index == 0 || index == 1);
            var startIdx = 2 * DirectionUtils.DirectionToIndex(dir);
            return Flags[startIdx + index];
        }

        public static bool AreGluedTogether(Vector2Int posA, TileSurface surfA, Vector2Int posB, TileSurface surfB)
        {
            var ab = posB - posA;
            var ba = posA - posB;

            Debug.Assert(ab.ManhattanLength() == 1);

            return SurfaceFlags.AreGluedTogether(surfA.GetFlags(ab, 0), surfB.GetFlags(ba, 1)) ||
                   SurfaceFlags.AreGluedTogether(surfA.GetFlags(ab, 1), surfB.GetFlags(ba, 0));
        }

        public static TileSurface Create(
            SurfaceFlagsEnum r0,
            SurfaceFlagsEnum r1,
            SurfaceFlagsEnum u0,
            SurfaceFlagsEnum u1,
            SurfaceFlagsEnum l0,
            SurfaceFlagsEnum l1,
            SurfaceFlagsEnum d0,
            SurfaceFlagsEnum d1)
        {
            var flags = new SurfaceFlags[]
            {
                new SurfaceFlags(r0),
                new SurfaceFlags(r1),
                new SurfaceFlags(u0),
                new SurfaceFlags(u1),
                new SurfaceFlags(l0),
                new SurfaceFlags(l1),
                new SurfaceFlags(d0),
                new SurfaceFlags(d1),
            };

            return new TileSurface(flags);
        }
        
        protected bool Equals(TileSurface other)
        {
            return Flags.SequenceEqual(other.Flags);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TileSurface) obj);
        }

        public override int GetHashCode()
        {
            return (Flags != null ? Flags.GetHashCode() : 0);
        }

    }
}