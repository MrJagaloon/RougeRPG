using System;
using UnityEngine;

namespace TileMapLib.BaseMaps
{
    public static class WhiteNoiseBMG
    {
        public static BaseMap<float> GenerateFloatNoise(int rows, int cols, FloatRange valueRange, System.Random rnd = null)
        {
            if (rnd == null)
            {
                int seed = System.DateTime.UtcNow.ToString().GetHashCode();
                rnd = new System.Random(seed);
            }

            BaseMap<float> baseMap = new BaseMap<float>(rows, cols, valueRange.min);
            for (int x = 0; x < cols; ++x)
            {
                for (int y = 0; y < rows; ++y)
                {
                    baseMap.SetCellValue(x, y, valueRange.min + ((float)rnd.NextDouble() * valueRange.Difference()));
                }
            }

            return baseMap;
        }

        public static BaseMap<int> GenerateIntNoise(int rows, int cols, IntRange valueRange, System.Random rnd = null)
        {
            if (rnd == null)
            {
                int seed = System.DateTime.UtcNow.ToString().GetHashCode();
                rnd = new System.Random(seed);
            }

            BaseMap<int> baseMap = new BaseMap<int>(rows, cols, valueRange.min);
            for (int x = 0; x < cols; ++x)
            {
                for (int y = 0; y < rows; ++y)
                {
                    baseMap.SetCellValue(x, y, rnd.Next(valueRange.min, valueRange.max + 1));
                }
            }

            return baseMap;
        }

        public static BaseMap<bool> GenerateBoolNoise(int rows, int cols, System.Random rnd = null)
        {
            if (rnd == null)
            {
                int seed = System.DateTime.UtcNow.ToString().GetHashCode();
                rnd = new System.Random(seed);
            }

            BaseMap<bool> baseMap = new BaseMap<bool>(rows, cols, false);
            for (int x = 0; x < cols; ++x)
            {
                for (int y = 0; y < rows; ++y)
                {
                    baseMap.SetCellValue(x, y, rnd.NextDouble() < 0.5);
                }
            }

            return baseMap;
        }
    }
}
