using System.Collections.Generic;
using UnityEngine;

namespace TileMapLib.TileMaps
{
    public class TileMap : MonoBehaviour
    {
        TileSlot[][] _slots;
        int _rows;
        int _cols;

        public List<object> baseMaps;

        public TileSlot[] this[int i] { get { return _slots[i]; } }
        public int Rows { get { return _rows; } }
        public int Cols { get { return _cols; } }

        public static TileMap NewTileMap(int rows, int cols, string name = null)
        {
            TileMap tileMap = new GameObject(name).AddComponent<TileMap>();
            tileMap.Init(rows, cols);
            return tileMap;
        }

        public void Init(int rows, int cols)
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
                    this[x][y] = TileSlot.NewTileSlot(position);
                    this[x][y].transform.parent = transform;
                }
            }

            baseMaps = new List<object>();
        }
    }
}
