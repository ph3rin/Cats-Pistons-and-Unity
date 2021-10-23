using System.Collections.Generic;

namespace CatProcessingUnit.Machineries
{
    public static class QueueExt
    {
        public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> range)
        {
            foreach (var obj in range)
            {
                queue.Enqueue(obj);
            }
        }
    }
}