using System.Collections.Generic;

namespace TileMapLib
{
    public class TileSlotZone
    {
        readonly List<TileSlot> slots;
        public int id;

        public int Count
        {
            get { return slots.Count; }
        }

        public TileSlot this[int i]
        {
            get { return slots[i]; }
            set { slots[i] = value; }
        }

        public TileSlotZone(int id)
        {
            this.id = id;
            slots = new List<TileSlot>();
        }

        public void AddSlot(TileSlot slot)
        {
            slot.zone = this;
            slots.Add(slot);
        }
    }
}
