using System;
namespace TileMapLib.Generators
{
    public static class BitMap
    {
        public static bool[][] Copy(bool[][] map)
        {
            int cols = map.Length;
            int rows = map[0].Length;

            bool[][] copy = new bool[cols][];

            for (int x = 0; x < cols; ++x)
            {
                copy[x] = new bool[rows];
                for (int y = 0; y < rows; ++y)
                {
                    copy[x][y] = map[x][y];
                }
            }

            return copy;
        }
    }
}
