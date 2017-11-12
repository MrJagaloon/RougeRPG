using System;
namespace TileMapLib.BaseMaps
{
    public interface ICellularAutomataRule
    {
        bool NextCellState(BaseMap<bool> map, int x, int y);
    }
}
