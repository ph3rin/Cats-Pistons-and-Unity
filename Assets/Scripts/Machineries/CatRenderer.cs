using System;
using System.Collections;
using CatProcessingUnit.GameManagement;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CatProcessingUnit.Machineries
{
    [RequireComponent(typeof(RegisterService))]
    public class CatRenderer : MachineryRenderer<Cat>, IPointerClickHandler
    {
        [SerializeField] private UnityEvent _onInteract;
        [SerializeField] private Transform _heartSpawnPoint;
        [SerializeField] private SpriteRenderer _heartPrefab;
        [SerializeField] private UnityEvent _onHappy;

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
            _onInteract.Invoke();
        }

        public Tween Happy()
        {
            var heart = Instantiate(_heartPrefab);
            heart.transform.SetParent(_heartSpawnPoint, true);
            heart.transform.localPosition = Vector3.zero;
            GetComponent<Animator>().SetTrigger("meow");
            _onHappy.Invoke();
            return DOTween.Sequence()
                .Join(heart.transform.DOLocalMove(Vector3.up * 0.8f, 0.5f))
                .Join(heart.DOFade(1.0f, 0.5f))
                .Append(DOVirtual.DelayedCall(1f, () => { }))
                .Append(heart.DOFade(0.0f, 0.5f))
                .OnComplete(() => Destroy(heart));
        }
    }
}