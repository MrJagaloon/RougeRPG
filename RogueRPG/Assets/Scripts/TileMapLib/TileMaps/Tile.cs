using System;
using UnityEngine;

namespace TileMapLib.TileMaps
{
    [RequireComponent(typeof(Transform))]
    public class Tile : MonoBehaviour
    {
        [HideInInspector]
        public TileSlot slot;

        public void ChangeSlots(TileSlot newSlot)
        {
            if (slot != null)
                slot.RemoveTile(this);
            slot = newSlot;
            transform.position = newSlot.position.ToVector2();
            transform.parent = slot.transform;
        }

        public static Tile InstatiateTile(GameObject tileObj, TileSlot slot = null)
        {
            Tile tile = Instantiate(tileObj).GetComponent<Tile>();
            if (slot != null)
                tile.ChangeSlots(slot);
            return tile;
        }
    }
}
