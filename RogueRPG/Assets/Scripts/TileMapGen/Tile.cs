using System;
using UnityEngine;

namespace TileMapGen
{
    [RequireComponent(typeof(Transform))]
    public abstract class Tile : MonoBehaviour
    {
        public TileSlot slot;

        public Sprite sprite;

        public virtual void ChangeSlots(TileSlot newSlot)
        {
            slot = newSlot;
            transform.position = slot.position;
        }
    }
}
