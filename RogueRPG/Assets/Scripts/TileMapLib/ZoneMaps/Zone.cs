using System.Collections.Generic;
using System.Collections;
using System;

namespace TileMapLib.ZoneMaps
{
    public class Zone : IEnumerable
    {
        readonly List<IntPoint2> cellPositions;

        public readonly int number;

        public int Count { get { return cellPositions.Count; } }

        public IntPoint2 this[int i]
        {
            get { return cellPositions[i]; }
        }

        public Zone(int number)
        {
            cellPositions = new List<IntPoint2>();
            this.number = number;
        }

        public int AddCellPosition(IntPoint2 cellPosition)
        {
            int index = -(cellPositions.BinarySearch(cellPosition)) - 1;

            if (index < 0)
                throw new Exception("Cell already exists in zone.");
            
            cellPositions.Insert(index, cellPosition);

            return index;
        }
        public int AddCellPosition(int x, int y)
        {
            return AddCellPosition(new IntPoint2(x, y));
        }

        public int GetCellPositionIndex(IntPoint2 cellPosition)
        {
            return cellPositions.BinarySearch(cellPosition);
        }
        public int GetCellPositionIndex(int x, int y)
        {
            return GetCellPositionIndex(new IntPoint2(x, y));
        }

        public void RemoveCellAt(int index)
        {
            cellPositions.RemoveAt(index);
        }

        public void RemoveCell(IntPoint2 position)
        {
            int cellIndex = GetCellPositionIndex(position);

            // Check that the cell is within this zone.
            if (cellIndex < 0)
                throw new Exception(string.Format("Cant remove cell at {0} because it is not within zone {1}.", position, number));

            RemoveCellAt(cellIndex);
        }
        public void RemoveCell(int x, int y)
        {
            RemoveCell(new IntPoint2(x, y));
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)cellPositions).GetEnumerator();
        }
    }
}
