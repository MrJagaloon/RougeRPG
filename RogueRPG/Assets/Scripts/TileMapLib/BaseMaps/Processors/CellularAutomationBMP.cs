using System;
using UnityEngine;

namespace TileMapLib.BaseMaps.Processors
{
    public static class CellularAutomationBMP
    {
        public static BaseMap Process(BaseMap originalMap, int steps, ICellularAutomataRule rule, System.Random rnd = null)
        {
            if (rnd == null)
            {
                int seed = DateTime.Now.GetHashCode();
                rnd = new System.Random(seed);
            }

            BaseMap map = originalMap.Copy();

            for (int i = 0; i < steps; ++i)
            {
                BaseMap oldMap = map.Copy();
                for (int x = 0; x < originalMap.cols; ++x)
                {
                    for (int y = 0; y < originalMap.rows; ++y)
                    {
                        map.SetPosition(x, y, rule.NextCellState(oldMap, x, y));
                    }
                }
            }

            return map;
        }
    }
}
