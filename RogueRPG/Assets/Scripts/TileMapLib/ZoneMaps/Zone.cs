using System.Collections.Generic;
using System.Collections;
using System;

namespace TileMapLib.ZoneMaps
{
    public class Zone : IEnumerable
    {
        readonly List<IntPoint2> positions;

        public readonly int zoneNumber;

        public int Count { get { return positions.Count; } }

        public IntPoint2 this[int i]
        {
            get { return positions[i]; }
        }

        public Zone(int zoneNumber)
        {
            positions = new List<IntPoint2>();
            this.zoneNumber = zoneNumber;
        }

        public int AddPosition(IntPoint2 position)
        {
            int index = -(positions.BinarySearch(position)) - 1;

            if (index < 0)
                throw new Exception("Node already exists.");
            
            positions.Insert(index, position);

            return index;
        }
        public int AddPosition(int x, int y)
        {
            return AddPosition(new IntPoint2(x, y));
        }

        public int GetPositionIndex(IntPoint2 position)
        {
            return positions.BinarySearch(position);
        }
        public int GetPositionIndex(int x, int y)
        {
            return GetPositionIndex(new IntPoint2(x, y));
        }

        public void RemovePositionAt(int index)
        {
            positions.RemoveAt(index);
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)positions).GetEnumerator();
        }
    }
}
