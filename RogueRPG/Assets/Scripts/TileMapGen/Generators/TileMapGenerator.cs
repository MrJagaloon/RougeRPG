using UnityEngine;

namespace TileMapLib.Generators
{
    public class TileMapGenerator : MonoBehaviour
    {
        public int rows;
        public int cols;

        public ITileMapGeneratorStep[] steps;

        public TileMap Generate()
        {
            TileMap map = new TileMap(rows, cols);

            if (steps == null)
                return map;

            for (int i = 0; i < steps.Length; ++i)
            {
                steps[i].ProcessTileMap(map);
            }

            return map;
        }
    }
}
