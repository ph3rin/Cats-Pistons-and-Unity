using CatProcessingUnit.GameManagement;
using CatProcessingUnit.LevelManagement;
using UnityEngine;
using UnityEngine.UI;

namespace CatProcessingUnit.UI
{
    public class LevelSelectBtn : MonoBehaviour
    {
        private LevelDescriptor _level;
        [SerializeField] private Text _text;

        public void SetLevel(LevelDescriptor level)
        {
            var levelManager = ServiceLocator.GetService<LevelManager>();
            var id = levelManager.GetLevelId(level);
            _level = level;
            _text.text = (id + 1).ToString();
        }

        public void Disable()
        {
            GetComponent<CanvasGroup>().alpha = 0.0f;
            GetComponent<Button>().interactable = false;
        }

        public void Enable()
        {
            GetComponent<CanvasGroup>().alpha = 1.0f;
            GetComponent<Button>().interactable = true;
        }
    }
}