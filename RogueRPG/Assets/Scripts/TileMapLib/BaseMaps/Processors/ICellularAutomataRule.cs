using System;
namespace TileMapLib.BaseMaps.Processors
{
    public interface ICellularAutomataRule
    {
        int NextCellState(BaseMap map, int x, int y);
    }
}
