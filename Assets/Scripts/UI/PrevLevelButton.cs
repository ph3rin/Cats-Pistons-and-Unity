using CatProcessingUnit.GameManagement;
using UnityEngine;
using UnityEngine.UI;

namespace CatProcessingUnit.UI
{
    public class PrevLevelButton : MonoBehaviour
    {
        private Button _button;
        private LegacyLevelManager _legacyLevelManager;
        private int _levelIdx;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _legacyLevelManager = ServiceLocator.GetService<LegacyLevelManager>();
            _levelIdx = _legacyLevelManager.GetCurrentLevelIndex();
            if (!_legacyLevelManager.HasPreviousLevel())
            {
                Destroy(gameObject);
            }
        }

        public void OnClick()
        {
            _legacyLevelManager.GoToPreviousLevel();
        }
    }
}