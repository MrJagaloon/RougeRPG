using System;

namespace TileMapLib.ZoneMaps
{
    public static class PruneZonesZMP
    {
        // Remove any zones whose count is not within keepRange.
        public static void PruneZones<T>(ZoneMap<T> zoneMap, IntRange keepRange)
        {
            foreach (Zone zone in zoneMap)
            {
                // Check if the number of positions in the zone is within range.
                if (!keepRange.IsInRange(zone.Count))
                {
                    // Remove the zone.
                    zoneMap.RemoveZone(zone.number);
                }
            }
        }

        // Fill and remove any zones whose count is not within keepRange.
        public static void PruneAndFillZones<T>(ZoneMap<T> zoneMap, IntRange keepRange, T fillValue)
        {
            foreach (Zone zone in zoneMap)
            {
                // Check if the number of positions in the zone is within range.
                if (!keepRange.IsInRange(zone.Count))
                {
                    // Remove the zone.
                    zoneMap.FillZone(zone.number, fillValue);
                    zoneMap.RemoveZone(zone.number);
                }
            }
        }
    }
}