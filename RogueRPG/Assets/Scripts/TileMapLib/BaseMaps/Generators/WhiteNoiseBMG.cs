using System;
using UnityEngine;

namespace TileMapLib.BaseMaps.Generators
{
    public static class WhiteNoiseBMG
    {
        public static BaseMap Generate(int rows, int cols, IntRange valueRange, System.Random rnd = null)
        {
            if (rnd == null)
            {
                int seed = System.DateTime.UtcNow.ToString().GetHashCode();
                rnd = new System.Random(seed);
            }

            BaseMap baseMap = new BaseMap(rows, cols, valueRange.min);
            for (int x = 0; x < cols; ++x)
            {
                for (int y = 0; y < rows; ++y)
                {
                    baseMap.SetPosition(x, y, rnd.Next(valueRange.min, valueRange.max + 1));
                }
            }

            return baseMap;
        }
    }
}
