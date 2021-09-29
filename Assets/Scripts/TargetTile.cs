using UnityEngine;

namespace CatProcessingUnit
{
    public class TargetTile : MonoBehaviour
    {
        [SerializeField] private int _index;

        private Workshop Workshop { get; set; }
        public int Index => _index;

        public void SetColor(Color color)
        {
            GetComponent<SpriteRenderer>().color = color;
        }
    }
}