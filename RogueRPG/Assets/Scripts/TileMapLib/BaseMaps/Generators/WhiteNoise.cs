using System;
using UnityEngine;

namespace TileMapLib.BaseMaps.Generators
{
    public class BoolWhiteNoise : IBaseMapGenerator<bool>
    {
        float _prob;

        public float Prob { get { return _prob; } }

        public BoolWhiteNoise(float prob)
        {
            _prob = prob;
        }

        public BaseMap<bool> Generate(int rows, int cols, System.Random rnd = null)
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
                    baseMap[x][y] = rnd.NextDouble() < Prob;
                }
            }

            return baseMap;
        }
    }

    public class IntWhiteNoise : IBaseMapGenerator<int>
    {
        int _min;
        int _max;

        public int Min { get { return _min; } }
        public int Max { get { return _max; } }

        public IntWhiteNoise(int min = 0, int max = 1)
        {
            if (min >= max)
                throw new System.ArgumentException("Min must be less than max.");
            
            _min = min;
            _max = max;
        }

        public BaseMap<int> Generate(int rows, int cols, System.Random rnd = null)
        {
            if (rnd == null)
            {
                int seed = System.DateTime.UtcNow.ToString().GetHashCode();
                rnd = new System.Random(seed);
            }

            BaseMap<int> baseMap = new BaseMap<int>(rows, cols, Min);
            for (int x = 0; x < cols; ++x)
            {
                for (int y = 0; y < rows; ++y)
                {
                    baseMap[x][y] = rnd.Next(Min, Max + 1);
                }
            }

            return baseMap;
        }
    }

    public class FloatWhiteNoise : IBaseMapGenerator<float>
    {
        float _min;
        float _max;

        public float Min { get { return _min; } }
        public float Max { get { return _max; } }

        public FloatWhiteNoise(int min = 0, int max = 1)
        {
            if (min >= max)
                throw new System.ArgumentException("Min must be less than max.");
            
            _min = min;
            _max = max;
        }

        public BaseMap<float> Generate(int rows, int cols, System.Random rnd = null)
        {
            if (rnd == null)
            {
                int seed = System.DateTime.UtcNow.ToString().GetHashCode();
                rnd = new System.Random(seed);
            }

            BaseMap<float> baseMap = new BaseMap<float>(rows, cols, Min);
            for (int x = 0; x < cols; ++x)
            {
                for (int y = 0; y < rows; ++y)
                {
                    baseMap[x][y] = Min + (float)rnd.NextDouble() * (Max - Min);
                }
            }

            return baseMap;
        }
    }
}
