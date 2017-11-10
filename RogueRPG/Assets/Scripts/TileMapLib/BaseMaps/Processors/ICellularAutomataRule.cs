using System;
namespace TileMapLib.BaseMaps.Processors
{
    public interface ICellularAutomataRule
    {
        bool NextCellState(BaseMap<bool> map, int x, int y);
    }
}
