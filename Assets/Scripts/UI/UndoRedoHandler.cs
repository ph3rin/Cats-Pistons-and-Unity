using CatProcessingUnit.GameManagement;
using CatProcessingUnit.Machineries;
using UnityEngine;

namespace CatProcessingUnit.UI
{
    public class UndoRedoHandler : MonoBehaviour
    {
        private void Update()
        {
            var level = TransitionManager.ActiveLevel;

            if (level == null || TransitionManager.I.State != GlobalState.GamePlay) return;
            
            if (Input.GetKeyDown(KeyCode.Z))
            {
                level.Undo();
            } 
            else if (Input.GetKeyDown(KeyCode.C))
            {
                level.Redo();
            }
        }
    }
}