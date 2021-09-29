using UnityEngine;

namespace CatProcessingUnit
{
    public class ColorTile : WorkshopTile
    {
        public Workshop Workshop { get; set; }

        [SerializeField] private int _index;

        public int Index => _index;

        public void SetColor(Color color)
        {
            GetComponent<SpriteRenderer>().color = color;
        }
    }
}