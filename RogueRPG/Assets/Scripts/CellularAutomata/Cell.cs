using System;

namespace CellAuto
{
    public enum CellState { EMPTY = 0, FILLED }

    public class Cell
    {
        public int x { get; private set; }
        public int y { get; private set; }

        public CellState state { get; private set; }
        public int zoneNumber;

        public Cell(int x, int y, CellState state = CellState.EMPTY, int zone = -1)
        {
            this.x = x;
            this.y = y;
            this.state = state;
            this.zoneNumber = zone;
        }

        public bool IsFilled()
        {
            return state == CellState.FILLED;
        }

        public void Fill()
        {
            state = CellState.FILLED;
        }

        public void Empty()
        {
            state = CellState.EMPTY;
        }

        public void SwitchState()
        {
            state = (state == CellState.EMPTY) ? CellState.FILLED : CellState.EMPTY;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Cell other = (Cell)obj;

            return x == other.x && y == other.y;
        }

        public override int GetHashCode()
        {
            return (int)(Math.Pow(x, (int)state + 2) * (Math.Pow(y, (int)state) + 1));
        }
    }
}
