using System;

namespace TileMapLib.TileMaps.Generators
{
    public interface ITileMapGenerator
    {
        TileMap Generate(int seed);
    }
}
