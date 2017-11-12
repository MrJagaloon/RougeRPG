using System.Collections;
using System.Collections.Generic;
using TileMapLib.BaseMaps;

namespace TileMapLib.ZoneMaps
{
    public class ZoneMap : BaseMap, IEnumerable
    {
        readonly List<Zone> zones;

        public int Count { get { return zones.Count; } }

        public ZoneMap(int rows, int cols) : base(rows, cols, -1)
        {
            zones = new List<Zone>();
        }

        /* Add a new zone to the ZoneMap and return its index.
         */
        public int NewZone()
        {
            int zoneNumber = zones.Count;
            zones.Add(new Zone(zoneNumber));

            return zoneNumber;
        }

        public override void SetPosition(IntPoint2 position, int value)
        {
            int zoneNumber = value;
            int curZoneNumber = map[position.x][position.y];

            // Check if that position is already owned by the zone at zoneNumber.
            if (curZoneNumber == zoneNumber)
                return;

            // Check if the position is already owned by another zone.
            if (curZoneNumber >= 0)
            {
                // Remove the position from its current zone.
                Zone curZone = zones[curZoneNumber];
                int positionIndex = curZone.GetPositionIndex(position);
                curZone.RemovePositionAt(positionIndex);
            }

            // Check if the position should be added to another zone.
            if (zoneNumber >= 0)
            {
                Zone newZone = zones[zoneNumber];
                newZone.AddPosition(position);
            }

            base.SetPosition(position, zoneNumber);
        }
        public override void SetPosition(int x, int y, int value)
        {
            SetPosition(new IntPoint2(x, y), value);
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)zones).GetEnumerator();
        }
    }
}
