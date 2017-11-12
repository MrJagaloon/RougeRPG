using System;
using System.Collections.Generic;
using TileMapLib.BaseMaps;

namespace TileMapLib.ZoneMaps
{
    public static class FloodFillZMG
    {
        /* Find and return all of the zones in baseMap using flood fill. Only considers positions 
         * with a value of validValue.
         */
        public static ZoneMap<T> Generate<T>(BaseMap<T> baseMap, T validValue)
        {
            ZoneMap<T> zoneMap = new ZoneMap<T>(baseMap);

            for (int x = 0; x < baseMap.cols; ++x)
            {
                for (int y = 0; y < baseMap.rows; ++y)
                {
                    IntPoint2 position = new IntPoint2(x, y);
                    // Check that the position is valid for a zone and is not already zoned.
                    if (baseMap.GetCellValue(position).Equals(validValue) 
                        && !zoneMap.CellIsZoned(position))
                    {
                        int zoneNumber = zoneMap.NewZone();
                        FloodFillZone(position, zoneNumber, zoneMap, validValue);
                    }
                }
            }

            return zoneMap;
        }

        static void FloodFillZone<T>(IntPoint2 position, int zoneNumber, ZoneMap<T> zoneMap, T validValue)
        {
            // Check that position is in bounds.
            if (position.x < 0 || position.x >= zoneMap.cols || position.y < 0 || position.y >= zoneMap.rows)
                return;

            // Check if position is valid for zone and that position is not already part of the zoneNumber.
            if (!zoneMap.baseMap.GetCellValue(position).Equals(validValue) 
                || zoneMap.GetCellZoneNumber(position) == zoneNumber)
                return;

            zoneMap.SetCellZone(position, zoneNumber);

            // Flood left, right, below, and above the cell at position.
            FloodFillZone(new IntPoint2(position.x - 1, position.y), zoneNumber, zoneMap, validValue);
            FloodFillZone(new IntPoint2(position.x + 1, position.y), zoneNumber, zoneMap, validValue);
            FloodFillZone(new IntPoint2(position.x, position.y - 1), zoneNumber, zoneMap, validValue);
            FloodFillZone(new IntPoint2(position.x, position.y + 1), zoneNumber, zoneMap, validValue);
        }
    }
}
