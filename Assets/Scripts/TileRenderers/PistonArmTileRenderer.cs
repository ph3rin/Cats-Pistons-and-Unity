using System.Collections;
using CatProcessingUnit.AnimationInstructions;
using CatProcessingUnit.TileDataNS;
using UnityEngine;

namespace CatProcessingUnit.TileRenderers
{
    public class PistonArmTileRenderer : RotatableTileRenderer
    {
        [SerializeField] private SpriteConfig _sprites;

        public void Render(PistonArmTileData tileData)
        {
            base.Render(tileData);
            _spriteRenderer.sprite = _sprites.GetPistonArmSprite(tileData.Sticky);
        }

        public override AnimationManager QueueAnimation(AnimationInstruction instruction)
        {
            base.QueueAnimation(instruction);

            var animator = GetComponent<Animator>();
            if (instruction is AnimationPush push)
            {
                animator.Play("Push");
                AnimationManager.Queue(WaitForAnimatorToFinish(animator));
            }
            else if (instruction is AnimationPull pull)
            {
                animator.Play("Pull");
                AnimationManager.Queue(WaitForAnimatorToFinish(animator));
            }
            return AnimationManager;
        }

        private IEnumerator WaitForAnimatorToFinish(Animator animator)
        {
            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        }
    }
}