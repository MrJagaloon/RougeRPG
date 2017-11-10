using System;

namespace TileMapLib.BaseMaps
{
    public class BaseMap<T>
    {
        int _rows;
        int _cols;
        T[][] map;

        public int Rows { get { return _rows; } }
        public int Cols { get { return _cols; } }
        public T[] this[int i]
        {
            get { return map[i]; }
            set { map[i] = value; }
        }

        public BaseMap (int rows, int cols, T initValue = default(T))
        {
            _rows = rows;
            _cols = cols;

            map = new T[cols][];
            for (int x = 0; x < Cols; ++x) 
            {
                map[x] = new T[rows];
                for (int y = 0; y < Rows; ++y)
                {
                    map[x][y] = initValue;
                }
            }
        }

        public BaseMap<T> Copy()
        {
            BaseMap<T> copy = new BaseMap<T>(Rows, Cols);

            for (int x = 0; x < Cols; ++x)
            {
                copy[x] = new T[Rows];
                for (int y = 0; y < Rows; ++y)
                {
                    copy[x][y] = map[x][y];
                }
            }

            return copy;
        }
    }
}
