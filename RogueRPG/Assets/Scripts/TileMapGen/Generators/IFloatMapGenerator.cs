using System;
namespace TileMapLib.Generators
{
    public interface IFloatMapGenerator
    {
        float[][] Generate(float[][] map, System.Random rnd = null);
    }
}
