using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CatProcessingUnit.Machineries
{
    public class PistonRenderer : MachineryRenderer<Piston>, IPointerClickHandler
    {
        [SerializeField] private int _maxLength;
        [SerializeField] private int _currentLength;

        [SerializeField] private Transform _headTransform;
        [SerializeField] private SpriteRenderer _stemRenderer;
        
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
            var elapsed = 0.0f;
            while (elapsed <= time)
            {
                var t = elapsed / time;
                transform.localPosition = Vector2.Lerp(src.Position, dest.Position, t);
                _headTransform.localPosition = Vector2.Lerp(
                    src.CurrentLength * Vector2.right,
                    dest.CurrentLength * Vector2.right,
                    t);
                _stemRenderer.size = Vector2.Lerp(
                    new Vector2(src.CurrentLength + 1, 1.0f),
                    new Vector2(dest.CurrentLength + 1, 1.0f),
                    t);
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            transform.localPosition = (Vector2) dest.Position;
            _headTransform.localPosition = dest.CurrentLength * Vector2.right;
            _stemRenderer.size = new Vector2(dest.CurrentLength + 1, 1.0f);
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