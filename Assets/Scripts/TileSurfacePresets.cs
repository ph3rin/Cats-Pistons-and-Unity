namespace CatProcessingUnit
{
    public static class TileSurfacePresets
    {
        public static readonly TileSurface Solid =
            TileSurface.Create(
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Exists);

        public static readonly TileSurface SolidSticky =
            TileSurface.Create(
                SurfaceFlagsEnum.Sticky,
                SurfaceFlagsEnum.Sticky,
                SurfaceFlagsEnum.Sticky,
                SurfaceFlagsEnum.Sticky,
                SurfaceFlagsEnum.Sticky,
                SurfaceFlagsEnum.Sticky,
                SurfaceFlagsEnum.Sticky,
                SurfaceFlagsEnum.Sticky);

        public static readonly TileSurface PistonSticky =
            TileSurface.Create(
                SurfaceFlagsEnum.Sticky,
                SurfaceFlagsEnum.Sticky,
                SurfaceFlagsEnum.Sticky,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Sticky);

        public static readonly TileSurface PistonExtended =
            TileSurface.Create(
                SurfaceFlagsEnum.Empty,
                SurfaceFlagsEnum.Empty,
                SurfaceFlagsEnum.Empty,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Empty);

        public static readonly TileSurface PistonArm =
            TileSurface.Create(
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Exists,
                SurfaceFlagsEnum.Empty,
                SurfaceFlagsEnum.Empty,
                SurfaceFlagsEnum.Empty,
                SurfaceFlagsEnum.Empty,
                SurfaceFlagsEnum.Exists);

        public static readonly TileSurface PistonArmSticky =
            TileSurface.Create(
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