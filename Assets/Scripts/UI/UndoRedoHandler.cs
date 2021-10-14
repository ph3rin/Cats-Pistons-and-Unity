using System;
using CatProcessingUnit.GameManagement;
using UnityEngine;

namespace CatProcessingUnit.UI
{
    public class UndoRedoHandler : MonoBehaviour
    {
        private Workshop _workshop;
        
        private void Start()
        {
            _workshop = ServiceLocator.GetService<Workshop>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                _workshop.Undo();
            } 
            else if (Input.GetKeyDown(KeyCode.C))
            {
                _workshop.Redo();
            }
        }
    }
}