using UnityEngine;
using TileMapLib.BaseMaps;
using TileMapLib.BaseMaps.Processors;
using TileMapLib.BaseMaps.Generators;
using TileMapLib.ZoneMaps;
using TileMapLib.ZoneMaps.Generators;
using TileMapLib.ZoneMaps.Processors;

namespace TileMapLib.TileMaps.Generators
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

            BaseMap fillMap = WhiteNoiseBMG.Generate(rows, cols, new IntRange(0, 1), rnd);

            for (int i = 0; i < rules.Length; ++i)
            {
                fillMap = CellularAutomationBMP.Process(fillMap, stepsPerRules[i], rules[i]);

            }

            TileMap map = TileMap.NewTileMap(rows, cols);

            ZoneMap caverns = new ZoneMap(rows, cols);
            caverns = FloodFillZMG.Generate(fillMap, 0);
            caverns = PruneZonesZMP.PruneZones(caverns, cavernSize);

            // Add floor and wall tiles
            for (int x = 0; x < cols; ++x)
            {
                for (int y = 0; y < rows; ++y)
                {
                    // Zoned positions are floors.
                    GameObject[] tileSet = caverns.GetPosition(x, y) >= 0 ? floorTileSet : wallTileSet;
                    int tileIndex = rnd.Next(tileSet.Length);
                    Tile.InstatiateTile(tileSet[tileIndex], map[x][y]);
                }
            }

            return map;
        }
    }
}
