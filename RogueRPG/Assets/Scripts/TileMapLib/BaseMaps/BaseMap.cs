using System;

namespace TileMapLib.BaseMaps
{
    public abstract class BaseMap
    {
        public readonly int rows;
        public readonly int cols;

        protected BaseMap(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
        }
    }

    public class BaseMap<T> : BaseMap
    {
        protected readonly T[][] cells;

        public BaseMap (int rows, int cols, T defaultValue = default(T)) : base(rows, cols)
        {
            cells = new T[cols][];
            for (int x = 0; x < cols; ++x) 
            {
                cells[x] = new T[rows];
                for (int y = 0; y < rows; ++y)
                {
                    cells[x][y] = defaultValue;
                }
            }
        }

        public void SetCellValue(IntPoint2 position, T value)
        {
            cells[position.x][position.y] = value;
        }
        public void SetCellValue(int x, int y, T value)
        {
            cells[x][y] = value;
        }

        public T GetCellValue(IntPoint2 position)
        {
            return cells[position.x][position.y];
        }
        public T GetCellValue(int x, int y)
        {
            return cells[x][y];
        }

        public BaseMap<T> Copy()
        {
            BaseMap<T> copy = new BaseMap<T>(rows, cols);

            for (int x = 0; x < cols; ++x)
            {
                for (int y = 0; y < rows; ++y)
                {
                    copy.SetCellValue(x, y, cells[x][y]);

                }
            }

            return copy;
        }
    }
}
