using System;

namespace TileMapLib.Generators
{
    public interface IBitMapGenerator
    {
        bool[][] Generate(bool[][] map, System.Random rnd = null);
    }
}
