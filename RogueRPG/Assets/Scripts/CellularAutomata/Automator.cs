using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace CellAuto
{
    [System.Serializable]
    public class Automator
    {
        public int ticks;

        public bool generateZones;
        public bool pruneZones;
        public bool connectZonesToMain;
        public int minEmptyZoneSize;
        public float emptyStepCost = 1f;
        public float filledStepCost = 10f;

        [Range(0f, 100f)]
        public float fillChance;

        public CANeighborRules fillRules;
        public CANeighborRules stayFilledRules;

        int rows;
        int cols;
        public int Rows { get { return rows; } }
        public int Cols { get { return cols; } }

        Cell[][] cells;

        List<Zone> zones;

        public Cell[] this[int key]
        {
            get { return cells[key]; }
            set { cells[key] = value; }
        }

        public bool IsPointInBounds(int x, int y)
        {
            return x >= 0 && x < cols && y >= 0 && y < rows;
        }

        /* Initializes and generates a matrix of cells using its fill and stay filled rules.
         */
        public void Generate(Random rnd)
        {
            InitCells(rnd);

            for (int i = 0; i < ticks; ++i)
            {
                Tick();
            }

            if (generateZones)
                GenerateEmptyZones(rnd);
        }

        /* Generates the cells with the given number rows and columns.
         */
        public void Generate(int rows, int cols, Random rnd)
        {
            this.rows = rows;
            this.cols = cols;
            Generate(rnd);
        }

        /* Initialize the matrix of cells, with each cell having a random chance at being born.
         */
        void InitCells(Random rnd)
        {
            // Generate initial cells
            cells = new Cell[cols][];
            for (int x = 0; x < cols; ++x)
            {
                cells[x] = new Cell[rows];
                for (int y = 0; y < rows; ++y)
                {
                    CellState state = (rnd.Next(0, 100) < fillChance) ? 
                        CellState.FILLED : CellState.EMPTY;

                    cells[x][y] = new Cell(x, y, state);
                }
            }
        }

        /* Perform a single cycle for each cell.
         */
        void Tick()
        {
            for (int x = 0; x < cells.Length; ++x)
            {
                for (int y = 0; y < cells[0].Length; ++y)
                {
                    // Check if empty cell should be filled
                    if (!cells[x][y].IsFilled() &&
                        fillRules[GetFilledNeighborCount(x, y, fillRules.ruleType)])
                        cells[x][y].Fill();

                    // Check if filled cell should be emptied
                    else if (cells[x][y].IsFilled() &&
                             !stayFilledRules[GetFilledNeighborCount(x, y, stayFilledRules.ruleType)])
                        cells[x][y].Empty();
                }
            }
        }

        /* Returns the number of neighbors of the cell at (cellX, cellY) that are alive. The 
         * boundaries are considered living. The ruleType determines which cells are considered
         * neighbors. VON_NEUMANN considers only adjacent orthoganal cells neighbors; MOORE considers 
         * both adjacent orthoganal cells and adjacent diagonal cells neighbors.
         */
        int GetFilledNeighborCount(int cellX, int cellY, RuleType ruleType)
        {
            int filledCount = 0;
            for (int adjacentX = cellX - 1; adjacentX <= cellX + 1; ++adjacentX)
            {
                for (int adjacentY = cellY - 1; adjacentY <= cellY + 1; ++adjacentY)
                {
                    switch (ruleType)
                    {
                        case RuleType.VON_NEUMANN:
                            if ((adjacentX == cellX && adjacentY != cellY) ||
                                (adjacentX != cellX && adjacentY == cellY))
                            {
                                // Check if adjacent cell is outside map
                                if (adjacentX < 0 || adjacentX >= cells.Length || 
                                    adjacentY < 0 || adjacentY >= cells[0].Length)
                                    filledCount++;
                                else
                                    filledCount += cells[adjacentX][adjacentY].IsFilled() ? 1 : 0;
                            }
                            break;
                        case RuleType.MOORE:
                            // Check if the adjacent cell is outside the bounds of the map.
                            if (adjacentX >= 0 && adjacentX < cells.Length && 
                                adjacentY >= 0 && adjacentY < cells[0].Length)
                            {
                                if (adjacentX != cellX || adjacentY != cellY)
                                    filledCount += cells[adjacentX][adjacentY].IsFilled() ? 1 : 0;
                            }
                            else
                                // Add one if the adjacent cell is outside the bounds of the map.
                                filledCount++;
                            break;
                    }
                }
            }

            return filledCount;
        }

        /* Finds, prunes, and then connects the zones.
         */
        void GenerateEmptyZones(Random rnd)
        {
            InitZones();

            if (pruneZones)
                PruneZones();
            
            if (connectZonesToMain)
                ConnectEmptyZonesToMain(rnd);
        }

        /* Finds all zones.
         */
        void InitZones()
        {
            // Initialize list of empty zones.
            zones = new List<Zone>();

            // Flood fill the empty cells to find the zones.
            int zoneNumber = 0;
            for (int x = 0; x < cols; ++x)
            {
                for (int y = 0; y < rows; ++y)
                {
                    Zone zone = FloodFillZone(cells[x][y], zoneNumber);
                    if (zone != null)
                    {
                        zones.Add(zone);
                        zoneNumber++;
                    }
                }
            }
            Debug.Log("Initalized " + zoneNumber + " zones");
        }

        /* Returns the zone that contains the cell.
         */
        Zone FloodFillZone(Cell c, int zoneNumber, Zone zone = null)
        {
            // Only flood empty, zoneless cells.
            if (c.IsFilled() || c.zoneNumber >= 0)
                return zone;

            if (zone == null)
                zone = new Zone(zoneNumber);

            // Add the cell to the deadZone.
            c.zoneNumber = zoneNumber;
            zone.Add(c);

            // Recursively call on the Von Neumann neighbors of the cell at (x, y).
            List<Cell> adjacentCells = GetAdjacentCells(c);
            foreach (Cell adjCell in adjacentCells)
            {
                zone = FloodFillZone(adjCell, zoneNumber, zone);
            }

            return zone;
        }

        /* Any zones with less empty cells than threshold are filled.
         */
        void PruneZones()
        {
            if (zones.Count == 0)
                return;

            string log = "";

            // Check that each zone has at least minEmptyZoneCells cells, and fill any that do not.
            for (int i = 0; i < zones.Count; ++i)
            {
                if (zones[i].Count < minEmptyZoneSize)
                {
                    zones[i].Fill();

                    // Nullify the zone to allow it to be removed later.
                    log += "Pruned zone " + zones[i].number + "\n";
                    zones[i] = null;
                }
            }

            // Remove all null zones.
            zones.RemoveAll(item => item == null);
            Debug.Log(log);
        }

        /* Creates pathways from all zones to the main zone
         */
        void ConnectEmptyZonesToMain(Random rnd)
        {
            int mainZoneIndex = GetMainEmptyZoneIndex();

            Zone mainZone = zones[mainZoneIndex];

            string log = "";
            for (int i = 0; i < zones.Count; ++i)
            {
                if (i != mainZoneIndex)
                {
                    Zone originZone = zones[i];
                    Cell originCell = originZone[rnd.Next(originZone.Count)];
                    Cell destinationCell = mainZone[rnd.Next(mainZone.Count)];
                    CreatePath(originCell, destinationCell);
                    log += "Connected zone " + i + " to main zone " + mainZoneIndex + "\n";
                }
            }
            Debug.Log(log);
        }

        void CreatePath(Cell origin, Cell destination)
        {
            List<PathStep<Cell>> openSteps = new List<PathStep<Cell>>();
            HashSet<PathStep<Cell>> closedSteps = new HashSet<PathStep<Cell>>();

            openSteps.Add(new PathStep<Cell>(origin));

            do
            {
                // Pop the next step in openSteps
                PathStep<Cell> curStep = openSteps.First();
                Cell curCell = curStep.data;
                openSteps.RemoveAt(0);

                closedSteps.Add(curStep);

                // Destination reached, carve the path.
                if (curStep.data.Equals(destination)) 
                {
                    do
                    {
                        if (curStep.parent != null)
                        {
                            curCell = curStep.data;
                            curCell.Empty();
                        }
                        curStep = curStep.parent;
                    } while (curStep != null);
                    return;
                }

                List<Cell> adjacentCells = GetAdjacentCells(curCell);

                // Perform A* search to find shortest path.
                foreach (Cell adjCell in adjacentCells)
                {
                    PathStep<Cell> step = new PathStep<Cell>(adjCell);
                    Cell stepCell = step.data;

                    // Only proceed if the nextStep is not already in the closedSteps.
                    if (!closedSteps.Contains(step))
                    {
                        float moveCost = GetCostForStep(curCell, stepCell);
                        step.parent = curStep;
                        step.gCost = curStep.gCost + moveCost;

                        // Check if the nextStep is already in the openSteps
                        int stepIndex = openSteps.IndexOf(step);

                        // nextStep is not in openSteps, so add it.
                        if (stepIndex == -1)
                        {
                            step.hCost = GetCostForStep(stepCell, destination);
                            InsertPathStep(step, openSteps);
                        }
                        // nextStep is already in openSteps, so update its score.
                        else
                        {
                            // If the current path gives nextStep a better gScore than its existing 
                            // gScore, remove the existing and insert step.
                            if (curStep.gCost + moveCost < openSteps[stepIndex].gCost)
                            {
                                
                                step.hCost = openSteps[stepIndex].hCost;
                                openSteps.RemoveAt(stepIndex);
                                InsertPathStep(step, openSteps);
                            }
                        }
                    }
                }
            } while (openSteps.Count > 0);
        }

        /* Inserts step into list at its sorted position. list is sorted by ascending fScore's.
         */
        void InsertPathStep(PathStep<Cell> step, List<PathStep<Cell>> list)
        {
            int i = 0;
            for (; i < list.Count; ++i)
            {
                if (step.fCost <= list[i].fCost)
                    break;
            }
            list.Insert(i, step);
        }

        /* Returns the hScore for the step from fromCell to toCell.
         */
        float GetCostForStep(Cell fromCell, Cell toCell)
        {
            float multiplier = (toCell.IsFilled()) ? filledStepCost : emptyStepCost;
            return multiplier * (Mathf.Abs(toCell.x - fromCell.x) + Mathf.Abs(toCell.y - fromCell.y));
        }

        /* Return a list with the Von Neumann adjacent cells coordinates
         */
        List<Cell> GetAdjacentCells(Cell cell)
        {
            List<Cell> adjCells = new List<Cell>();

            // Left
            if (IsPointInBounds(cell.x - 1, cell.y))
                adjCells.Add(cells[cell.x - 1][cell.y]);

            // Right
            if (IsPointInBounds(cell.x + 1, cell.y))
                adjCells.Add(cells[cell.x + 1][cell.y]);

            // Up
            if (IsPointInBounds(cell.x, cell.y + 1))
                adjCells.Add(cells[cell.x][cell.y + 1]);

            // Down
            if (IsPointInBounds(cell.x, cell.y - 1))
                adjCells.Add(cells[cell.x][cell.y - 1]);

            return adjCells;
        }

        /* Returns the index of the main zone, which is the largest zone
         */
        int GetMainEmptyZoneIndex()
        {
            int largestEZIndex = -1;
            int largestEZSize = 0;
            for (int i = 0; i < zones.Count; ++i)
            {
                int count = zones[i].Count;
                if (count > largestEZSize)
                {
                    largestEZSize = count;
                    largestEZIndex = i;
                }
            }
            return largestEZIndex;
        }
    }
}
