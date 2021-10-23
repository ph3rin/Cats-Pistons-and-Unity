using UnityEngine;

namespace CatProcessingUnit.Machineries
{
    public interface IMachineryRenderer
    {
        IMachineryHistory CreateMachineryHistory(LevelHistory levelHistory);
    }
}