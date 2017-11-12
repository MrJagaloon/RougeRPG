using System;
using UnityEngine;

namespace TileMapLib.BaseMaps
{
    public static class CellularAutomationBMP
    {
        public static BaseMap<bool> Process(BaseMap<bool> originalMap, int steps, ICellularAutomataRule rule, System.Random rnd = null)
        {
            if (rnd == null)
            {
                int seed = DateTime.Now.GetHashCode();
                rnd = new System.Random(seed);
            }

            BaseMap<bool> map = originalMap.Copy();

            for (int i = 0; i < steps; ++i)
            {
                BaseMap<bool> oldMap = map.Copy();
                for (int x = 0; x < originalMap.cols; ++x)
                {
                    for (int y = 0; y < originalMap.rows; ++y)
                    {
                        map.SetCellValue(x, y, rule.NextCellState(oldMap, x, y));
                    }
                }
            }

            return map;
        }
    }
}
