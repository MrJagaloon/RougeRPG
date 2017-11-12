using System;
using System.Collections;
using System.Collections.Generic;
using TileMapLib.BaseMaps;

namespace TileMapLib.ZoneMaps
{
    public class ZoneMap : BaseMap, IEnumerable
    {
        protected readonly List<Zone> zones;
        protected readonly int[][] zoneMap;

        int count;
        public int Count { get { return count; } }

        public ZoneMap(int rows, int cols) : base(rows, cols)
        {
            count = 0;
            zones = new List<Zone>();

            zoneMap = new int[cols][];
            for (int x = 0; x < cols; ++x)
            {
                zoneMap[x] = new int[rows];
                for (int y = 0; y < rows; ++y)
                {
                    zoneMap[x][y] = -1;
                }
            }
        }

        /* Add a new zone to the ZoneMap and return its index.
         */
        public int NewZone()
        {
            int zoneNumber = zones.Count;
            zones.Add(new Zone(zoneNumber));

            count++;

            return zoneNumber;
        }

        public void RemoveZone(int zoneNumber)
        {
            Zone zone = zones[zoneNumber];

            foreach (IntPoint2 cellPosition in zone)
            {
                zoneMap[cellPosition.x][cellPosition.y] = -1;
            }

            zones[zoneNumber] = null;
            count--;
        }

        public void UnzoneCell(IntPoint2 position)
        {
            int curZoneNumber = zoneMap[position.x][position.y];

            // Check if the cell is currently zoned.
            if (ZoneExists(curZoneNumber))
            {
                zones[curZoneNumber].RemoveCell(position);
            }

            zoneMap[position.x][position.y] = -1;
        }
        public void UnzoneCell(int x, int y)
        {
            UnzoneCell(new IntPoint2(x, y));
        }

        public void SetCellZone(IntPoint2 position, int zoneNumber)
        {
            // Check that the zone exists.
            if (!ZoneExists(zoneNumber))
                throw new Exception(string.Format("Zone number {0} does not exist", zoneNumber));


            int curZoneNumber = zoneMap[position.x][position.y];

            // Check if that position is already owned by the zone at zoneNumber.
            if (curZoneNumber == zoneNumber)
                return;

            // Check if the position is already owned by another zone.
            if (curZoneNumber >= 0)
            {
                // Remove the position from its current zone.
                Zone curZone = zones[curZoneNumber];
                int cellIndex = curZone.GetCellPositionIndex(position);
                curZone.RemoveCellAt(cellIndex);
            }

            // Check if the position should be added to another zone.
            if (zoneNumber >= 0)
            {
                Zone newZone = zones[zoneNumber];
                newZone.AddCellPosition(position);
            }

            zoneMap[position.x][position.y] = zoneNumber;
        }
        public void SetCellZone(int x, int y, int zoneNumber)
        {
            SetCellZone(new IntPoint2(x, y), zoneNumber);
        }

        public int GetCellZoneNumber(IntPoint2 position)
        {
            return zoneMap[position.x][position.y];
        }
        public int GetCellZoneNumber(int x, int y)
        {
            return zoneMap[x][y];
        }

        public Zone GetCellZone(IntPoint2 position)
        {
            int zoneNumber = GetCellZoneNumber(position);
            if (zoneNumber < 0)
                return null;
            return zones[zoneNumber];
        }
        public Zone GetCellZone(int x, int y)
        {
            return GetCellZone(new IntPoint2(x, y));
        }

        public bool CellIsZoned(IntPoint2 position)
        {
            return GetCellZoneNumber(position) >= 0;
        }
        public bool CellIsZoned(int x, int y)
        {
            return CellIsZoned(new IntPoint2(x, y));
        }

        public bool ZoneExists(int zoneNumber)
        {
            return zoneNumber >= 0 && zoneNumber < zones.Count && zones[zoneNumber] != null;
        }

        public IEnumerator GetEnumerator()
        {
            return new ZoneEnumerator(zones);
        }
    }

    public class ZoneMap<T> : ZoneMap
    {
        public BaseMap<T> baseMap;

        public ZoneMap(BaseMap<T> baseMap) : base(baseMap.rows, baseMap.cols)
        {
            this.baseMap = baseMap;
        }

        public void FillZone(int zoneNumber, T fillValue)
        {
            Zone fillZone = zones[zoneNumber];

            foreach (IntPoint2 cellPosition in fillZone)
            {
                baseMap.SetCellValue(cellPosition, fillValue);
            }
        }
    }


    public class ZoneEnumerator : IEnumerator
    {
        public List<Zone> zones;
        int position = -1;

        public ZoneEnumerator(List<Zone> zones)
        {
            this.zones = zones;
        }

        object IEnumerator.Current { get { return Current; } }
        public Zone Current
        {
            get
            {
                try
                {
                    return zones[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public bool MoveNext()
        {
            do
            {
                position++;
            } while (position < zones.Count && zones[position] == null);
            return position < zones.Count;
        }

        public void Reset()
        {
            position = -1;
        }
    }
}
