using System;
using System.Collections.Generic;
using TileMapLib.BaseMaps;

namespace TileMapLib.ZoneMaps.Generators
{
    public static class FloodFillZMG
    {
        /* Find and return all of the zones in baseMap using flood fill. Only considers positions 
         * with a value of validValue.
         */
        public static ZoneMap Generate(BaseMap baseMap, int validValue)
        {
            ZoneMap zoneMap = new ZoneMap(baseMap.rows, baseMap.cols);

            for (int x = 0; x < baseMap.cols; ++x)
            {
                for (int y = 0; y < baseMap.rows; ++y)
                {
                    // Check that the position is valid for a zone and is not already zoned.
                    if (baseMap.GetPosition(x, y) == validValue && zoneMap.GetPosition(x, y) < 0)
                    {
                        int zoneNumber = zoneMap.NewZone();
                        FloodFillZone(x, y, zoneNumber, zoneMap, baseMap, validValue);
                    }
                }
            }

            return zoneMap;
        }

        static void FloodFillZone(int x, int y, int zoneNumber, ZoneMap zoneMap, BaseMap baseMap, int validValue)
        {
            // Check if position is valid for zone and that position is not already part of the zoneNumber.
            if (baseMap.GetPosition(x, y) != validValue || zoneMap.GetPosition(x, y) == zoneNumber)
                return;

            zoneMap.SetPosition(x, y, zoneNumber);

            // Flood left
            if (x > 0)
                FloodFillZone(x - 1, y, zoneNumber, zoneMap, baseMap, validValue);

            // Flood right
            if (x < zoneMap.cols - 1)
                FloodFillZone(x + 1, y, zoneNumber, zoneMap, baseMap, validValue);

            // Flood down
            if (y > 0)
                FloodFillZone(x, y - 1, zoneNumber, zoneMap, baseMap, validValue);

            // Flood up
            if (y < zoneMap.rows - 1)
                FloodFillZone(x, y + 1, zoneNumber, zoneMap, baseMap, validValue);
        }
    }
}
