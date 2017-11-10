using UnityEngine;
using TileMapLib.BaseMaps;
using TileMapLib.BaseMaps.Processors;
using TileMapLib.BaseMaps.Generators;

namespace TileMapLib.TileMaps.Generators
{
    public class RougeCaveTileMapGenerator : MonoBehaviour, ITileMapGenerator
    {
        public int rows = 32;
        public int cols = 32;

        public int[] stepsPerRules;
        public MooresRule[] rules;

        [Range(0.0f, 1.0f)]
        public float initFullProb = 0.5f;

        public GameObject[] floorTileSet;
        public GameObject[] wallTileSet;


        public TileMap Generate(int seed)
        {
            if (stepsPerRules.Length != rules.Length)
                throw new System.Exception("stepsPerRules.Length must equal rules.Length");
            System.Random rnd = new System.Random(seed);

            BoolWhiteNoise boolWhiteNoise = new BoolWhiteNoise(initFullProb);
            BaseMap<bool> fillMap = boolWhiteNoise.Generate(rows, cols, rnd);

            CellularAutomation cellAutomator = new CellularAutomation();
            for (int i = 0; i < rules.Length; ++i)
            {
                fillMap = cellAutomator.Generate(fillMap, stepsPerRules[i], rules[i]);
            }

            TileMap map = TileMap.NewTileMap(rows, cols);

            // Add floor and wall tiles
            for (int x = 0; x < cols; ++x)
            {
                for (int y = 0; y < rows; ++y)
                {
                    GameObject[] tileSet = fillMap[x][y] ? wallTileSet : floorTileSet;
                    int tileIndex = rnd.Next(tileSet.Length);
                    Tile.InstatiateTile(tileSet[tileIndex], map[x][y]);
                }
            }

            return map;
        }
    }
}
