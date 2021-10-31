using CatProcessingUnit.GameManagement;
using CatProcessingUnit.Machineries;
using UnityEngine;

namespace CatProcessingUnit.UI
{
    public class UndoRedoHandler : MonoBehaviour
    {
        private LevelHistory _levelHistory;
        
        private void Start()
        {
            _levelHistory = ServiceLocator.GetService<LevelHistory>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                _levelHistory.Undo();
            } 
            else if (Input.GetKeyDown(KeyCode.C))
            {
                _levelHistory.Redo();
            }
        }
    }
}