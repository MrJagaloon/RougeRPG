using System;
namespace TileMapLib.BaseMaps.Generators
{
    public interface IBaseMapGenerator<T>
    {
        BaseMap<T> Generate(int rows, int cols, System.Random rnd = null);
    }
}
