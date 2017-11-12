using UnityEngine;
using TileMapLib.BaseMaps;
using TileMapLib.ZoneMaps;

namespace TileMapLib.TileMaps
{
    public class RougeCaveTileMapGenerator : MonoBehaviour
    {
        public int rows = 32;
        public int cols = 32;
        public IntRange cavernSize;

        public int[] stepsPerRules;
        public MooresCAR[] rules;

        [Range(0.0f, 1.0f)]
        public float initFullProb = 0.5f;

        public GameObject[] floorTileSet;
        public GameObject[] wallTileSet;


        public TileMap Generate(int seed)
        {
            if (stepsPerRules.Length != rules.Length)
                throw new System.Exception("stepsPerRules.Length must equal rules.Length");
            System.Random rnd = new System.Random(seed);

            BaseMap<bool> fillMap = WhiteNoiseBMG.GenerateBoolNoise(rows, cols, rnd);

            for (int i = 0; i < rules.Length; ++i)
            {
                fillMap = CellularAutomationBMP.Process(fillMap, stepsPerRules[i], rules[i]);
            }

            TileMap map = TileMap.NewTileMap(rows, cols);

            ZoneMap<bool> caverns = FloodFillZMG.Generate(fillMap, false);
            Debug.Log(caverns.Count);
            PruneZonesZMP.PruneAndFillZones(caverns, cavernSize, true);
            Debug.Log(caverns.Count);

            // Add floor and wall tiles
            for (int x = 0; x < cols; ++x)
            {
                for (int y = 0; y < rows; ++y)
                {
                    // Zoned positions are floors.
                    GameObject[] tileSet = fillMap.GetCellValue(x, y) ? wallTileSet : floorTileSet;
                    int tileIndex = rnd.Next(tileSet.Length);
                    Tile.InstatiateTile(tileSet[tileIndex], map[x][y]);
                }
            }

            return map;
        }
    }
}
