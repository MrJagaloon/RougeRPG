using System.Collections.Generic;
using UnityEngine;

namespace Board
{
    [RequireComponent(typeof(Transform))]
    public class Cell : MonoBehaviour
    {
        public IntPoint2 position;

        public Zone currentZone;

        public List<Tile> tiles;
        public int Count { get { return tiles.Count; } }
        public Tile this[int i]
        {
            get { return tiles[i]; }
            set { tiles[i] = value; }
        }

        public static Cell NewCell(IntPoint2 position)
        {
            GameObject cellGO = new GameObject(string.Format("Cell({0}, {1})", position.x, position.y));
            Cell cell = cellGO.AddComponent<Cell>();
            cell.position = position;
            cell.tiles = new List<Tile>();
            cell.transform.position = new Vector3(x, y, 0f);
            return cell;
        }
        public static Cell NewCell(int x, int y)
        {
            return NewCell(new IntPoint2(x, y));
        }

        public void AddTile(Tile tile)
        {
            tiles.Add(tile);
            tile.ChangedCell(this);     // TODO: implement tile.ChangedTile into tile base class
            tile.cell = this;
            tile.transform.parent = transform;
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
