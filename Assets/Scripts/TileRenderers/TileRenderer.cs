using System;
using System.Collections;
using CatProcessingUnit.AnimationInstructions;
using CatProcessingUnit.GameManagement;
using CatProcessingUnit.TileDataNS;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CatProcessingUnit.TileRenderers
{
    public class TileRenderer : MonoBehaviour, IPointerClickHandler
    {
        protected SpriteRenderer _spriteRenderer;
        private AnimationManager _animationManager;

        protected AnimationManager AnimationManager
        {
            get
            {
                if (_animationManager == null) _animationManager = ServiceLocator.GetService<AnimationManager>();
                return _animationManager;
            }
        }

        public event Action onLeftClick;
        public event Action onRightClick;

        protected virtual void Awake()
        {
            _animationManager = null;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            Debug.Assert(_spriteRenderer != null, "_spriteRenderer != null");
        }

        public void Render(TileData tileData)
        {
            transform.localPosition = new Vector2(tileData.Position.x, tileData.Position.y);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                onLeftClick?.Invoke();
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                onRightClick?.Invoke();
            }
        }

        public virtual AnimationManager QueueAnimation(AnimationInstruction instruction)
        {
            if (instruction is AnimationTranslate translate)
            {
                AnimationManager.Queue(CrtTranslate(translate.Offset, 1.0f, 31));
            }

            return AnimationManager;
        }


        private IEnumerator CrtTranslate(Vector2Int offset, float duration, int steps)
        {
            var src = (Vector2) transform.localPosition;
            var dest = src + offset;
            var stepSize = duration / steps;
            var time = 0.0f;

            while (true)
            {
                var currentStep = Mathf.FloorToInt(time / stepSize);
                var t = currentStep * stepSize / duration;
                transform.localPosition = Vector2.Lerp(src, dest, t);
                time += Time.deltaTime;
                if (currentStep == steps)
                {
                    yield break;
                }

                yield return null;
            }
        }
    }
}