using System.Collections.Generic;
using UnityEngine;

namespace TileMapGen
{
    [RequireComponent(typeof(Transform))]
    public class TileSlot : MonoBehaviour
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

        public static TileSlot NewTileSlot(IntPoint2 position)
        {
            GameObject cellGO = new GameObject(string.Format("TileSlot({0}, {1})", position.x, position.y));
            TileSlot cell = cellGO.AddComponent<TileSlot>();
            cell.position = position;
            cell.tiles = new List<Tile>();
            cell.transform.position = new Vector3(x, y, 0f);
            return cell;
        }
        public static TileSlot NewCell(int x, int y)
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

        public void RemoveTile(Tile tile)
        {
            bool wasRemoved = tiles.Remove(tile);

            // Check that the tile was in the list.
            if (wasRemoved == false)
                throw new System.Exception("tile");
        }

        public void RemoveTileAt(int i)
        {
            tiles.RemoveAt(i);
        }

        public int GetTileIndex(Tile tile)
        {
            for (int i = 0; i < tiles.Count; ++i)
            {
                if (tiles[i] == tile)
                    return i;
            }
            return -1;
        }

        public override bool Equals(object other)
        {
            if (other == null || GetType() != other.GetType())
                return false;

            TileSlot otherCell = (TileSlot)other;

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
