using System;
using CatProcessingUnit.GameManagement;
using CatProcessingUnit.LevelManagement;
using UnityEngine;
using UnityEngine.UI;

namespace CatProcessingUnit.UI
{
    public class NextLevelButton : MonoBehaviour
    {
        private bool NextLevelExists()
        {
            var levelManager = ServiceLocator.GetService<LevelManager>();
            var nextLevel = levelManager.GetLevel(levelManager.GetCurrentLevelId() + 1);
            return nextLevel != null;
        }
        
        private void Start()
        {
            if (!NextLevelExists())
            {
                GetComponentInChildren<Text>().text = "You win! MEOW :)";
            }
        }

        public void LoadNextLevel()
        {
            if (NextLevelExists())
            {
                ServiceLocator.GetService<LevelManager>().LoadNextLevel();
            }
            else
            {
                Debug.LogError("MEOW~~~~~~~~~~~~~~~~~~");
                ServiceLocator.GetService<LevelManager>().LoadLevel(0);

            }
        }
    }
}