namespace CatProcessingUnit.Machineries
{
    public interface ICloneable<out T>
    {
        public T Clone();
    }
}