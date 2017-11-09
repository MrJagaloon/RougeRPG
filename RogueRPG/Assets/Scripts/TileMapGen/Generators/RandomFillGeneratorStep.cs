using UnityEngine;

namespace TileMapLib.Generators
{
    public class RandomFillGeneratorStep : ITileMapGeneratorStep
    {
        public GameObject[] emptyTileSet;
        public GameObject[] filledTileSet;

        public RandomFillGeneratorStep(GameObject[] emptyTileSet, GameObject[] filledTileSet)
        {
            this.emptyTileSet = emptyTileSet;
            this.filledTileSet = filledTileSet;
        }

        public void ProcessTileMap(TileMap map, System.Random rnd = null, params int[] baseMapsIndexes)
        {
            if (rnd == null)
            {
                int seed = System.DateTime.UtcNow.ToString().GetHashCode();
                rnd = new System.Random(seed);
            }

            int fillMapIndex = baseMapsIndexes[0];
            bool[][] fillMap = (bool[][])map.baseMaps[fillMapIndex];

            int cols = fillMap.Length;
            int rows = fillMap[0].Length;

            for (int x = 0; x < fillMap.Length; ++x)
            {
                for (int y = 0; y < fillMap.Length; ++y)
                {
                    GameObject[] tileSet = fillMap[x][y] ? filledTileSet : emptyTileSet;
                    int tileIndex = rnd.Next(tileSet.Length);

                    Tile tile = GameObject.Instantiate(tileSet[tileIndex], Vector3.zero, Quaternion.identity).GetComponent<Tile>();
                    tile.ChangeSlots(map[x][y]);
                }
            }
        }
    }
}
