using System.Collections.Generic;

namespace CatProcessingUnit.Machineries
{
    public class MachineryHistory<T> : IMachineryHistory where T : Machinery
    {
        private readonly List<T> _history = new List<T>();
        private readonly MachineryRenderer<T> _renderer;
        private readonly LevelHistory _levelHistory;

        public MachineryHistory(MachineryRenderer<T> renderer, LevelHistory levelHistory)
        {
            _renderer = renderer;
            _history.Add(renderer.Current);
            _levelHistory = levelHistory;
        }
    }
}