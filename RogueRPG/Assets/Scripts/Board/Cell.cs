using System.Collections.Generic;
using UnityEngine;

namespace Board
{
    [RequireComponent(typeof(Transform))]
    public class Cell : MonoBehaviour
    {
        public int x { get; private set; }
        public int y { get; private set; }

        public int zoneNumber;

        public List<Tile> tiles;
        public int Count { get { return tiles.Count; } }
        public Tile this[int i]
        {
            get { return tiles[i]; }
            set { tiles[i] = value; }
        }

        public bool IsBlocking 
        {
            get
            {
                foreach (Tile t in tiles)
                    if (t.isBlocking) return true;
                return false;
            }
        }

        public static Cell NewCell(int x, int y)
        {
            GameObject cellGO = new GameObject("Cell(" + x + ", " + y + ")");
            Cell cell = cellGO.AddComponent<Cell>();
            cell.x = x;
            cell.y = y;
            cell.tiles = new List<Tile>();
            cell.transform.position = new Vector3(x, y, 0f);
            return cell;
        }

        public void AddTile(Tile tile, bool moveToCell = true)
        {
            tiles.Add(tile);
            tile.cell = this;
            tile.transform.parent = transform;
            if (moveToCell)
                tile.transform.position = transform.position;
        }

        public void AddTile(GameObject tileGO, bool moveToCell = true)
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

        public override bool Equals(object other)
        {
            if (other == null || GetType() != other.GetType())
                return false;

            Cell otherCell = (Cell)other;

            return x == otherCell.x && y == otherCell.y;
        }

        public override int GetHashCode()
        {
            return x * y + (x + 1) * (y + 1) * (y + 2);
        }

        public override string ToString()
        {
            return string.Format("[Cell: ({0}, {1})]", x, y);
        }
    }
}
