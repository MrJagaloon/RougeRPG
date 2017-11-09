
namespace TileMapLib.Generators
{
    public static class WhiteNoise
    {
        public static float[][] GenerateFloatMap(int rows, int cols, System.Random rnd = null)
        {
            if (rnd == null)
            {
                int seed = System.DateTime.UtcNow.ToString().GetHashCode();
                rnd = new System.Random(seed);
            }

            float[][] map = new float[cols][];
            for (int x = 0; x < cols; ++x)
            {
                map[x] = new float[rows];
                for (int y = 0; y < rows; ++y)
                {
                    map[x][y] = (float)rnd.NextDouble();
                }
            }
            return map;
        }

        public static bool[][] GenerateBitMap(int rows, int cols, System.Random rnd = null)
        {
            if (rnd == null)
            {
                int seed = System.DateTime.UtcNow.ToString().GetHashCode();
                rnd = new System.Random(seed);
            }

            bool[][] map = new bool[cols][];
            for (int x = 0; x < cols; ++x)
            {
                map[x] = new bool[rows];
                for (int y = 0; y < rows; ++y)
                {
                    map[x][y] = (float)rnd.NextDouble() > 0.5f;
                }
            }
            return map;
        }
    }
}
