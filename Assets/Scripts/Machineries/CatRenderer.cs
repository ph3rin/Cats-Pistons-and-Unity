using System;
using System.Collections;
using CatProcessingUnit.GameManagement;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CatProcessingUnit.Machineries
{
    [RequireComponent(typeof(RegisterService))]
    public class CatRenderer : MachineryRenderer<Cat>, IPointerClickHandler, IService
    {
        [SerializeField] private Text _happyText;
        [SerializeField] private float _happyTextFadeInTime;

        private void Awake()
        {
            _happyText.gameObject.SetActive(false);
        }

        protected override Cat CreateMachineryInternal()
        {
            return new Cat(Vector2Int.RoundToInt(transform.localPosition));
        }

        public override IEnumerator LerpTowards(Cat dest, float time)
        {
            var src = CurrentMachinery;
            CurrentMachinery = dest;
            var elapsed = 0.0f;
            while (elapsed <= time)
            {
                var t = elapsed / time;
                transform.localPosition = Vector2.Lerp(src.Position, dest.Position, t);
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            transform.localPosition = (Vector2) dest.Position;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"MEOW {Time.frameCount}!!!");
        }

        public Tweener FadeInHappyText()
        {
            _happyText.gameObject.SetActive(true);
            var oldColor = _happyText.color;
            _happyText.color = new Color(oldColor.r, oldColor.g, oldColor.b, 0.0f);
            return _happyText.DOFade(1.0f, _happyTextFadeInTime);
        }
        
        public void Init()
        {
        }
    }
}