using UnityEngine;

namespace CatProcessingUnit.AnimationInstructions
{
    public class AnimationTranslate : AnimationInstruction
    {
        public Vector2Int Offset { get; }

        public AnimationTranslate(Vector2Int offset)
        {
            Offset = offset;
        }
    }
}