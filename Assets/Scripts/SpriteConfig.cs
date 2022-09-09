using UnityEngine;

namespace CatProcessingUnit
{
    [CreateAssetMenu(menuName = "CPU/Piston Sprite Config", order = 100)]
    public class SpriteConfig : ScriptableObject
    {
        [SerializeField] private Sprite _retractedSprite;
        [SerializeField] private Sprite _retractedStickySprite;
        [SerializeField] private Sprite _extendedSprite;
        [SerializeField] private Sprite _extendedStickySprite;
        [SerializeField] private Sprite _arm;
        [SerializeField] private Sprite _armSticky;
        [SerializeField] private Sprite _armStem;

        public Sprite GetPistonSprite(bool sticky, bool extended)
        {
            return extended
                ? (sticky ? _extendedStickySprite : _extendedSprite)
                : (sticky ? _retractedStickySprite : _retractedSprite);
        }

        public Sprite GetPistonArmSprite(bool sticky, bool isStem)
        {
            if (isStem) return _armStem;
            return sticky ? _armSticky : _arm;
        }
    }
}