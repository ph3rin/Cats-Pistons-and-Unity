using System;
using UnityEngine;
using UnityEngine.UI;

namespace CatProcessingUnit
{
    public class SelectLevelBtnText : MonoBehaviour
    {
        private void Update()
        {
            var state = TransitionManager.I.State;
            if (state == GlobalState.GamePlay)
            {
                GetComponent<Text>().text = "Levels";
            }
            else if (state == GlobalState.LevelSelection)
            {
                GetComponent<Text>().text = "Play";
            }
        }
    }
}