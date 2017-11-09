using System;

namespace TileMapLib.Generators
{
    public interface ITileMapGeneratorStep
    {
        // Returns a copy of map that has been processed.
        void ProcessTileMap(TileMap map, System.Random rnd = null, params int[] baseMapsIndexes);
    }
}
