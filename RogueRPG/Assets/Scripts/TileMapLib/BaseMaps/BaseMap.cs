using System;

namespace TileMapLib.BaseMaps
{
    public class BaseMap
    {
        public readonly int rows;
        public readonly int cols;
        protected readonly int[][] map;

        public BaseMap (int rows, int cols, int defaultValue = 0)
        {
            this.rows = rows;
            this.cols = cols;

            map = new int[cols][];
            for (int x = 0; x < cols; ++x) 
            {
                map[x] = new int[rows];
                for (int y = 0; y < rows; ++y)
                {
                    map[x][y] = defaultValue;
                }
            }
        }

        public virtual void SetPosition(IntPoint2 position, int value)
        {
            map[position.x][position.y] = value;
        }
        public virtual void SetPosition(int x, int y, int value)
        {
            map[x][y] = value;
        }

        public virtual int GetPosition(IntPoint2 position)
        {
            return map[position.x][position.y];
        }
        public virtual int GetPosition(int x, int y)
        {
            return map[x][y];
        }

        public BaseMap Copy()
        {
            BaseMap copy = new BaseMap(rows, cols);

            for (int x = 0; x < cols; ++x)
            {
                for (int y = 0; y < rows; ++y)
                {
                    copy.SetPosition(x, y, map[x][y]);
                }
            }

            return copy;
        }
    }
}
