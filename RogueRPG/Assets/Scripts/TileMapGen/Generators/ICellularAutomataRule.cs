using System;
namespace TileMapLib.Generators
{
    // TODO: Implement Von Neumann and Moore rules.
    public interface ICellularAutomataRule
    {
        bool ProcessCell(bool[][] map, int x, int y);
    }
}
