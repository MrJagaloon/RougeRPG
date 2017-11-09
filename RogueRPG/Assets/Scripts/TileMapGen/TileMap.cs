using System;

namespace TileMapGen
{
    public class TileMap
    {
        readonly TileSlot _slots;
        int _rows;
        int _cols;

        public TileSlot[] this[int i] { get { return slots[i]; } }
        public int Rows { get { return _rows; } }
        public int Cols { get { return _cols; } }

        public TileMap(int rows, int cols)
        {
            _rows = rows;
            _cols = cols;

            _slots = new TileSlot[Cols][];
            for (int x = 0; x < Cols; ++x)
            {
                _slots[x] = new TileSlot[Rows];
                for (int y = 0; y < Cols; ++y)
                {
                    IntPoint2 position = new IntPoint2(x, y);
                    _slots[x][y] = TileSlot.NewTileSlot(position);
                }
            }
        }
    }
}
