using System;
using System.Collections.Generic;
using System.Linq;
using CatProcessingUnit.GameManagement;
using CatProcessingUnit.LevelManagement;
using UnityEngine;

namespace CatProcessingUnit.UI
{
    public class LevelSelectionPanel : MonoBehaviour
    {
        private LevelManager _levelManager;
        private List<LevelSelectBtn> _buttons;
        private int _midIndex;
        private int _pageId;

        private void Start()
        {
            _levelManager = ServiceLocator.GetService<LevelManager>();
            _buttons = GetComponentsInChildren<LevelSelectBtn>().ToList();
            _buttons.Sort((lhs, rhs) =>
                lhs.transform.GetSiblingIndex().CompareTo(rhs.transform.GetSiblingIndex()));

            var currentLevelId = _levelManager.GetCurrentLevelId();
            var pageId = currentLevelId / _buttons.Count;
            SetPageId(pageId);
        }

        private void SetPageId(int pageId)
        {
            var levels = Enumerable.Range(0, _buttons.Count)
                .Select(i => i + _buttons.Count * pageId)
                .Select(_levelManager.GetLevel).ToList();
            if (levels.All(l => l == null))
            {
                return;
            }

            for (var i = 0; i < _buttons.Count; ++i)
            {
                var button = _buttons[i];
                var level = levels[i];
                if (level != null)
                {
                    button.Enable();
                    button.SetLevel(level);
                }
                else
                {
                    button.Disable();
                }
            }
        }
    }
}