using System.Collections.Generic;
using UnityEngine;

namespace Board
{
    [RequireComponent(typeof(Transform))]
    public class Cell : MonoBehaviour
    {
        int _x;
        int _y;

        public int x 
        { 
            get { return _x; }
            set 
            { 
                _x = value;
                Vector3 newPos = transform.position;
                newPos.x = _x;
                transform.position = newPos;
                gameObject.name = "Cell(" + x + ", " + y + ")";
            }
        }
        public int y
        {
            get { return _y; }
            set
            {
                _y = value;
                Vector3 newPos = transform.position;
                newPos.y = _y;
                transform.position = newPos;
                gameObject.name = "Cell(" + x + ", " + y + ")";
            }
        }

        public static Cell NewCell(int x, int y)
        {
            GameObject cellGO = new GameObject();
            Cell cell = cellGO.AddComponent<Cell>();
            cell.x = x;
            cell.y = y;
            cell.tiles = new List<Tile>();
            return cell;
        }

        public int zoneNumber;

        public List<Tile> tiles;
        public int Count { get { return tiles.Count; } }
        public Tile this[int i]
        {
            get { return tiles[i]; }
            set { tiles[i] = value; }
        }

        public void AddTile(Tile tile, bool moveToCell = false)
        {
            tiles.Add(tile);
            if (moveToCell)
                tile.transform.position = transform.position;
        }

        public void AddTile(GameObject tileGO, bool moveToCell = false)
        {
            AddTile(tileGO.GetComponent<Tile>(), moveToCell);
        }

        public void RemoveTile(Tile tile)
        {
            bool wasRemoved = tiles.Remove(tile);

            // Check that the tile was in the list.
            if (wasRemoved == false)
                throw new System.Exception("tile");
        }

        public void RemoveTile(int i)
        {
            tiles.RemoveAt(i);
        }

        public int GetTileIndex(GameObject tile)
        {
            for (int i = 0; i < tiles.Count; ++i)
            {
                if (tiles[i] == tile)
                    return i;
            }
            return -1;
        }

        public void Collision(ICollider c)
        {
            foreach (Tile tile in tiles)
            {
                tile.Collision(c);
            }
        }
    }
}
