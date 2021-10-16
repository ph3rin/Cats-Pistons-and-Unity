using CatProcessingUnit.AnimationInstructions;
using CatProcessingUnit.TileDataNS;

namespace CatProcessingUnit
{
    public class DeferredAnimationInstruction
    {
        public AnimationInstruction Instruction { get; }
        public TileData Tile { get;  }

        public DeferredAnimationInstruction(AnimationInstruction instruction, TileData tile)
        {
            Instruction = instruction;
            Tile = tile;
        }
    }
}