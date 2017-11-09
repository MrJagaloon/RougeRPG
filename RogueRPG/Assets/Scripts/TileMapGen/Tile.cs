using System;
using UnityEngine;

namespace TileMapLib
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
    }
}
