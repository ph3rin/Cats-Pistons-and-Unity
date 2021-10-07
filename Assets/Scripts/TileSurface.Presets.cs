namespace CatProcessingUnit
{
    public partial class TileSurface
    {
        public static readonly TileSurface Solid =
            Create(
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Exists);

        public static readonly TileSurface SolidSticky =
            Create(
                SurfaceFlagsEnum.Sticky,
                SurfaceFlagsEnum.Sticky,
                SurfaceFlagsEnum.Sticky,
                SurfaceFlagsEnum.Sticky,
                SurfaceFlagsEnum.Sticky,
                SurfaceFlagsEnum.Sticky,
                SurfaceFlagsEnum.Sticky,
                SurfaceFlagsEnum.Sticky);

        public static readonly TileSurface PistonSticky =
            Create(
                SurfaceFlagsEnum.Sticky,
                SurfaceFlagsEnum.Sticky,
                SurfaceFlagsEnum.Sticky,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Sticky);

        public static readonly TileSurface PistonExtended =
            Create(
                SurfaceFlagsEnum.Empty,
                SurfaceFlagsEnum.Empty,
                SurfaceFlagsEnum.Empty,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Empty);

        public static readonly TileSurface PistonArm =
            Create(
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Empty,
                SurfaceFlagsEnum.Empty,
                SurfaceFlagsEnum.Empty,
                SurfaceFlagsEnum.Empty,
                SurfaceFlagsEnum.Exists);

        public static readonly TileSurface PistonArmSticky =
            Create(
                SurfaceFlagsEnum.Sticky,
                SurfaceFlagsEnum.Sticky,
                SurfaceFlagsEnum.Sticky,
                SurfaceFlagsEnum.Empty,
                SurfaceFlagsEnum.Empty,
                SurfaceFlagsEnum.Empty,
                SurfaceFlagsEnum.Empty,
                SurfaceFlagsEnum.Sticky);
    }
}