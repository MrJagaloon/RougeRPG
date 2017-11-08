using System.Collections.Generic;

namespace CellAuto
{
    public class Zone
    {
        List<Cell> cells;
        public int number;

        public int Count
        {
            get { return cells.Count; }
        }

        public Cell this[int i]
        {
            get { return cells[i]; }
            set { cells[i] = value; }
        }

        public Zone(int number)
        {
            this.number = number;
            cells = new List<Cell>();
        }

        public void Add(Cell cell)
        {
            cell.zoneNumber = number;
            cells.Add(cell);
        }

        public void Fill()
        {
            foreach (Cell c in cells)
            {
                c.Fill();
            }
            cells = null;
        }
    }
}
