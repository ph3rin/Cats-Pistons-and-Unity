using System.Collections;
using UnityEngine;

namespace CatProcessingUnit.Machineries
{
    public class GlassRenderer : MachineryRenderer<Glass>
    {
        protected override Glass CreateMachineryInternal()
        {
            return new Glass(Vector2Int.RoundToInt(transform.localPosition));
        }

        public override IEnumerator LerpTowards(Glass dest, float time)
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
    }
}