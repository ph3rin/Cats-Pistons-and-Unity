using System;
using System.Collections.Generic;
using CatProcessingUnit.GameManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CatProcessingUnit
{
    [RequireComponent(typeof(RegisterService))]
    public class LevelManager : MonoBehaviour, IService
    {
        [Serializable]
        private class Level
        {
            public string Name;
            public bool Completed;
            public SceneCollection SceneCollection;
        }

        [SerializeField] private List<Level> _levels;

        public void Init()
        {
        }

        public int GetCurrentLevelIndex()
        {
            // todo: this is a hack, there should be a proper way to do this
            int index = 0;
            var stringSplit = SceneManager.GetActiveScene().name.Split(' ');
            if (stringSplit.Length >= 2)
                int.TryParse(stringSplit[1], out index);
            return Mathf.Clamp(index - 1, 0, 32);
        }

        public string GetCurrentLevelName()
        {
            return _levels[GetCurrentLevelIndex()].Name;
        }

        public bool HasNextLevel()
        {
            return GetCurrentLevelIndex() < _levels.Count - 1;
        }

        public bool HasPreviousLevel()
        {
            return GetCurrentLevelIndex() > 0;
        }

        public void GoToNextLevel()
        {
            GameManager.LoadSceneCollection(_levels[GetCurrentLevelIndex() + 1].SceneCollection);
        }

        public void GoToPreviousLevel()
        {
            GameManager.LoadSceneCollection(_levels[GetCurrentLevelIndex() - 1].SceneCollection);
        }

        public bool IsLevelCompleted(int index)
        {
            return _levels[index].Completed;
        }

        public void CompleteCurrentLevel()
        {
            _levels[GetCurrentLevelIndex()].Completed = true;
        }
    }
}