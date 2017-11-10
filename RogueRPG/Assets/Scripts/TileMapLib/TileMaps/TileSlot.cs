using System.Collections.Generic;
using System;
using UnityEngine;

namespace TileMapLib.TileMaps
{
    [RequireComponent(typeof(Transform))]
    public class TileSlot : MonoBehaviour, IComparable, IComparable<TileSlot>
    {
        public IntPoint2 position;

        public TileSlotZone zone;

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
            TileSlot slot = cellGO.AddComponent<TileSlot>();
            slot.position = position;
            slot.tiles = new List<Tile>();
            slot.transform.position = new Vector3(position.x, position.y, 0f);
            return slot;
        }
        public static TileSlot NewCell(int x, int y)
        {
            return NewTileSlot(new IntPoint2(x, y));
        }

        public void AddTile(Tile tile)
        {
            tiles.Add(tile);
            tile.ChangeSlots(this);     // TODO: implement tile.ChangedTile into tile base class
        }

        public void RemoveTile(Tile tile)
        {
            bool wasRemoved = tiles.Remove(tile);

            // Check that the tile was in the list.
            if (wasRemoved == false)
                throw new System.Exception("tile");

            tile.slot = null;
            tile.transform.parent = null;
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

            return position.x == otherCell.position.x && position.y == otherCell.position.y;
        }

        public int CompareTo(TileSlot other)
        {
            return position.CompareTo(other.position);
        }

        public int CompareTo(object obj)
        {
            if (obj != null && !(obj is TileSlot))
                throw new ArgumentException("Object must be of type TileSlot.");

            return CompareTo((TileSlot)obj);
        }

        public override int GetHashCode()
        {
            return position.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("[Cell: ({0}, {1})]", position.x, position.y);
        }
    }
}
