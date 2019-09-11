namespace CERuntimeNodeGraph.Code
{
    public class RNG
    {
        public static readonly RNG_ClassWrap.LogicRoot Logic = new RNG_ClassWrap.LogicRoot();
        public static readonly RNG_ClassWrap.DisplayRoot Display = new RNG_ClassWrap.DisplayRoot();
        public static RNG_IAssetsFactory Assets;
    }
}