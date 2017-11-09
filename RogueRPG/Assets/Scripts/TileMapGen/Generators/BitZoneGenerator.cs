using System.Collections.Generic;

namespace TileMapLib.Generators
{
    public static class BitZoneGenerator
    {
        // TODO: Find bit zones by flood filling.
        public static List<BitZone> FindBitZones(bool[][] map, bool state)
        {
            return null;
        }
    }

    /* A bit zone is a group of contgous bits. Bits are stored as an int array storing 3 ints: the
     * x and y coordinate of the bit, and its state (false = 0, true = 1).
     */
    public class BitZone
    {
        List<int[]> bits;

        public BitZone()
        {
            bits = new List<int[]>();
        }

        public void AddBit(int x, int y, bool s)
        {
            int[] bit = { x, y, s ? 1 : 0 };
            bits.Add(bit);
        }

        public void RemoveBit(int x, int y)
        {
            for (int i = 0; i < bits.Count; ++i)
            {
                int[] bit = bits[i];
                if (BitX(bit) == x && BitY(bit) == y)
                {
                    bits.RemoveAt(i);
                }
            }
        }

        public bool GetBit(int i)
        {
            return BitState(bits[i]);
        }

        static int BitX(int[] bit)
        {
            return bit[0];
        }

        static int BitY(int[] bit)
        {
            return bit[1];
        }

        static bool BitState(int[] bit)
        {
            return bit[2] == 1 ? true : false;
        }
    }
}
