using CatProcessingUnit.GameManagement;
using UnityEngine;
using UnityEngine.UI;

namespace CatProcessingUnit.UI
{
    public class PrevLevelButton : MonoBehaviour
    {
        private Button _button;
        private LevelManager _levelManager;
        private int _levelIdx;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _levelManager = ServiceLocator.GetService<LevelManager>();
            _levelIdx = _levelManager.GetCurrentLevelIndex();
            if (!_levelManager.HasPreviousLevel())
            {
                Destroy(gameObject);
            }
        }

        public void OnClick()
        {
            _levelManager.GoToPreviousLevel();
        }
    }
}