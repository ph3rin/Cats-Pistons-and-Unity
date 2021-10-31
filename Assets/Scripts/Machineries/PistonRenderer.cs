using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace CatProcessingUnit.Machineries
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PistonRenderer : MachineryRenderer<Piston>, IPointerClickHandler
    {
        [SerializeField] private int _maxLength;
        [SerializeField] private int _currentLength;
        
        [FormerlySerializedAs("_headTransform")] [SerializeField] private SpriteRenderer _headRenderer;
        [SerializeField] private SpriteRenderer _stemRenderer;

        [SerializeField] private Sprite _baseSpriteOn;
        [SerializeField] private Sprite _baseSpriteOff;
        [SerializeField] private Sprite _headSpriteOn;
        [SerializeField] private Sprite _headSpriteOff;

        private SpriteRenderer _baseRenderer;
        
        private void Awake()
        {
            Debug.Assert(_maxLength >= 1);
            Debug.Assert(_currentLength <= _maxLength);
            _baseRenderer = GetComponent<SpriteRenderer>();
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
            _headRenderer.sprite = dest.IsSticky ? _headSpriteOn : _headSpriteOff;
            _baseRenderer.sprite = dest.IsSticky ? _baseSpriteOn : _baseSpriteOff;
            var elapsed = 0.0f;
            while (elapsed <= time)
            {
                var t = elapsed / time;
                transform.localPosition = Vector2.Lerp(src.Position, dest.Position, t);
                _headRenderer.transform.localPosition = Vector2.Lerp(
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
            _headRenderer.transform.localPosition = dest.CurrentLength * Vector2.right;
            _stemRenderer.size = new Vector2(dest.CurrentLength + 1, 1.0f);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (MachineryHistory.LevelHistory.State != GameState.Gameplay) return;
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                CurrentMachinery.Extend(MachineryHistory);
            } else if (eventData.button == PointerEventData.InputButton.Right)
            {
                CurrentMachinery.SetStickiness(MachineryHistory, !CurrentMachinery.IsSticky);
            }
        }
    }
}