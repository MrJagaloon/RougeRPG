using System;

namespace TileMapLib.ZoneMaps.Processors
{
    public static class PruneZonesZMP
    {
        // Remove any zones whose position count is not within keepRange.
        public static ZoneMap PruneZones(ZoneMap zoneMap, IntRange keepRange)
        {
            ZoneMap prunedZoneMap = new ZoneMap(zoneMap.rows, zoneMap.cols);
            foreach (Zone zone in zoneMap)
            {
                // Check if the number of positions in the zone is within range.
                if (keepRange.IsInRange(zone.Count))
                {
                    // Keep the zone
                    int zoneNumber = prunedZoneMap.NewZone();
                    foreach (IntPoint2 position in zone)
                    {
                        prunedZoneMap.SetPosition(position, zoneNumber);
                    }
                }
            }
            return prunedZoneMap;
        }
    }
}