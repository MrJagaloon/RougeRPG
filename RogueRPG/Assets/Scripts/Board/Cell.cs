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

        public void AddTile(Tile tile, bool moveToCell = false)
        {
            tiles.Add(tile);
            tile.transform.parent = transform;
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
