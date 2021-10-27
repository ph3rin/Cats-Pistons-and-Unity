using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CatProcessingUnit.Machineries
{
    public class CatRenderer : MachineryRenderer<Cat>, IPointerClickHandler
    {
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
    }
}