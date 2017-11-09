using System;
namespace TileMapLib.Generators
{
    public class CellularAutomation : IBitMapGenerator
    {
        bool[][] map;

        public int steps = 4;

        public ICellularAutomataRule[] rules;

        public int Rows { get { return map[0].Length; } }
        public int Cols { get { return map.Length; } }


        public bool[][] Generate(bool[][] originalMap, System.Random rnd = null)
        {
            map = BitMap.Copy(originalMap);

            for (int i = 0; i < steps; ++i)
            {
                ProcessStep();
            }

            return map;
        }

        /* Process each cell through each rule.
         */
        void ProcessStep()
        {
            for (int i = 0; i < rules.Length; ++i)
            {
                bool[][] oldMap = BitMap.Copy(map);

                for (int x = 0; x < Cols; ++x)
                {
                    for (int y = 0; y < Rows; ++y)
                    {
                        map[x][y] = rules[i].ProcessCell(oldMap, x, y);
                    }
                }
            }
        }
    }
}
