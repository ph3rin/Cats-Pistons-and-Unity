using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CatProcessingUnit.Machineries
{
    public class PistonRenderer : MachineryRenderer<Piston>, IPointerClickHandler
    {
        [SerializeField] private int _maxLength;
        [SerializeField] private int _currentLength;

        private void Awake()
        {
            Debug.Assert(_maxLength >= 1);
            Debug.Assert(_currentLength <= _maxLength);
        }

        protected override Piston CreateMachineryInternal()
        {
            return new Piston(
                Vector2Int.RoundToInt(transform.position),
                RotationUtil.RightVectorToDirection(transform.right),
                _maxLength,
                _currentLength);
        }

        public override IEnumerator LerpTowards(Piston dest, float time)
        {
            var src = CurrentMachinery;
            CurrentMachinery = dest;
            var t = 0.0f;
            while (t <= time)
            {
                transform.localPosition = Vector2.Lerp(src.Position, dest.Position, t / time);
                t += Time.deltaTime;
                yield return null;
            }
            
            transform.localPosition = (Vector2) dest.Position;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                CurrentMachinery.Extend(MachineryHistory);
            }
        }
    }
}