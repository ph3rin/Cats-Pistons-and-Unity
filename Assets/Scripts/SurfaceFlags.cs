using UnityEngine;

namespace CatProcessingUnit
{
    public readonly struct SurfaceFlags
    {
        public bool Exists { get; }
        public bool Sticky { get; }

        public SurfaceFlags(bool exists, bool sticky)
        {
            // Sticky should imply exists
            Debug.Assert(!sticky || exists);

            Exists = exists;
            Sticky = sticky;
        }

        public SurfaceFlags(SurfaceFlagsEnum flags) : this(
            flags != SurfaceFlagsEnum.Empty,
            flags == SurfaceFlagsEnum.Sticky)
        {
        }

        public static bool AreGluedTogether(SurfaceFlags a, SurfaceFlags b)
        {
            return a.Exists && b.Exists && (a.Sticky || b.Sticky);
        }
        
        public bool Equals(SurfaceFlags other)
        {
            return Exists == other.Exists && Sticky == other.Sticky;
        }

        public override bool Equals(object obj)
        {
            return obj is SurfaceFlags other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Exists.GetHashCode() * 397) ^ Sticky.GetHashCode();
            }
        }
    }

    public enum SurfaceFlagsEnum
    {
        Empty = 0,
        Exists = 1,
        Sticky = 3
    }
}