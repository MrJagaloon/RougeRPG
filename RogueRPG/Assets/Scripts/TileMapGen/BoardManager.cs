//using UnityEngine;
//using System.Collections.Generic;
//using System.Linq;

//namespace Board
//{
//    public class BoardManager : MonoBehaviour
//    {
//        public int maxEdgeSize;
//        public float maxAspectRatio;
//        public int edgeSize;
//        public float edgeChanceMod;

//        public int exitSpawnAttempts = 25;
//        public int enemySpawnAttempts = 100;
//        public int foodSpawnCycleAttempts = 10;
//        public int foodSpawnAttempts = 100;

//        public IntRange exitDistanceRange;
//        public IntRange foodRange;

//        public GameObject entryTilePrefab; 
//        public GameObject exitTilePrefab;
//        public GameObject[] floorTiles;
//        public GameObject[] wallTiles;
//        public GameObject[] outerWallTiles;
//        public GameObject[] foodTiles;
//        public GameObject[] enemyTiles;

//        public CellAuto.Automator wallMap;

//        public int cols { get; private set; }
//        public int rows { get; private set; }

//        Transform boardContainer;      // Container for board tiles.
//        Transform enemyContainer;

//        Cell[][] cells;
//        TileZone[] zones;

//        SpawnTile entryTile;
//        ExitTile exitTile;

//        /* Surround the board with random outerWallTiles and fill with random floorTiles.
//         */
//        void InitBoard()
//        {
//            // Get random size.
//            float aspectRatio = (int)GameManager.instance.Rnd.Next(100, (int)maxAspectRatio * 100) / 100f;
//            if (GameManager.instance.Rnd.Next(0, 100) > 50)
//            {
//                cols = maxEdgeSize;
//                rows = (int)(cols / aspectRatio);
//            }
//            else
//            {
//                rows = maxEdgeSize;
//                cols = (int)((1f / aspectRatio) * rows);
//            }

//            Debug.Log("Generating board with\n    Aspect Ratio: " + aspectRatio + "\n    Rows: " + rows + "\n    Cols: " + cols);
//        }

//        void InitCells()
//        {
//            // Initialize containers
//            boardContainer = new GameObject("Board").transform;
//            enemyContainer = new GameObject("Enemies").transform;
//            Transform outerWallTilesContainer = new GameObject("Outer Wall Tiles").transform;
//            outerWallTilesContainer.transform.parent = boardContainer;

//            cells = new Cell[cols][];
//            for (int x = -edgeSize; x < cols + edgeSize; ++x)
//            {
//                if (IsPointInBounds(x, 0))
//                    cells[x] = new Cell[rows];
//                for (int y = -edgeSize; y < rows + edgeSize; ++y)
//                {
//                    if (IsPointInBounds(x, y))
//                    {
//                        // Point is in bounds, so fill it with cells with random floor tiles.

//                        // Instatiate random floor tile.
//                        int floorIndex = GameManager.instance.Rnd.Next(floorTiles.Length);
//                        GameObject floorInstance =
//                            Instantiate(floorTiles[floorIndex], new Vector3(x, y, 0f), Quaternion.identity);

//                        // Instantiate cell.
//                        Cell cell = Cell.NewCell(x, y);
//                        cell.transform.parent = boardContainer;
//                        cell.AddTile(floorInstance, true);
//                        cells[x][y] = cell;
//                    }
//                    else
//                    {
//                        // Point is out of bounds, so fill it with outer wall tiles.
//                        // Spawn the outer walls randomly with a chance proportional to the distance 
//                        // to the nearest edge of the board. 

//                        float distance = 0f;

//                        if (x < 0 && y < 0)                 // Bottom left
//                            distance = Mathf.Sqrt(Mathf.Pow(-x - 1, 2) + Mathf.Pow(-y - 1, 2));
//                        else if (x < 0 && y >= rows)        // Top left
//                            distance = Mathf.Sqrt(Mathf.Pow(-x - 1, 2) + Mathf.Pow(y - rows, 2));
//                        else if (x < 0)                     // Left
//                            distance = -x - 1;
//                        else if (x >= cols && y < 0)        // Bottom right
//                            distance = Mathf.Sqrt(Mathf.Pow(x - cols, 2) + Mathf.Pow(-y - 1, 2));
//                        else if (x >= cols && y >= rows)     // Top right
//                            distance = Mathf.Sqrt(Mathf.Pow(x - cols, 2) + Mathf.Pow(y - rows, 2));
//                        else if (x >= cols)                 // Right
//                            distance = x - cols;
//                        else if (y < 0)                     // Bottom
//                            distance = -y - 1;
//                        else if (y >= rows)                 // Top
//                            distance = y - rows;

//                        float chance = (1f - distance / edgeSize) * edgeChanceMod;

//                        if (GameManager.instance.Rnd.Next(100) < chance * 100f)
//                        {
//                            int wallIndex = GameManager.instance.Rnd.Next(outerWallTiles.Length);
//                            GameObject wallInstance =
//                                Instantiate(outerWallTiles[wallIndex], new Vector3(x, y, 0f), Quaternion.identity);
//                            wallInstance.transform.parent = outerWallTilesContainer;
//                        }
//                    }
//                }
//            }
//        }

//        /* Initalize the board and fill with procedurally generated walls, enemies, and others.
//         */
//        public void SetupScene(int level)
//        {
//            InitBoard();
//            InitCells();

//            wallMap.Generate(rows, cols, GameManager.instance.Rnd);

//            // Convert wallMap's cellular automata zones to board zones.
//            zones = new TileZone[wallMap.zones.Count];
//            for (int i = 0; i < wallMap.zones.Count; ++i)
//            {
//                CellAuto.Zone wallMapZone = wallMap.zones[i];
//                TileZone zone = new TileZone(wallMapZone.number);
//                for (int j = 0; j < wallMapZone.Count; ++j)
//                {
//                    CellAuto.Cell wallMapCell = wallMapZone[j];
//                    Cell cell = cells[wallMapCell.x][wallMapCell.y];
//                    zone.Add(cell);
//                }
//                zones[i] = zone;
//            }

//            /* Loop through each cell of the wallMap. If it is "alive", place a random wall at that 
//             * cell's position on the board.
//             */
//            for (int x = 0; x < cols; ++x)
//            {
//                for (int y = 0; y < rows; ++y)
//                {
//                    if (wallMap[x][y].IsFilled())
//                    {
//                        GameObject wallToInstatiate = wallTiles[GameManager.instance.Rnd.Next(wallTiles.Length)];
//                        GameObject wallInstance = Instantiate(wallToInstatiate, Vector3.zero, Quaternion.identity);
//                        wallInstance.transform.parent = boardContainer;
//                        cells[x][y].AddTile(wallInstance, true);
//                    }
//                }
//            }

//            PlaceEntryAndExit();
//            PlaceFood();

//            // Generate enemies.
//            int enemyCount = (int)Mathf.Log(level, 2) * GameManager.instance.difficulty;
//            for (int i = 0; i < enemyCount; ++i)
//            {
//                int x, y;
//                int spawnAttempt = 0;
//                bool isValid = false;

//                do
//                {
//                    x = GameManager.instance.Rnd.Next(cols);
//                    y = GameManager.instance.Rnd.Next(rows);
//                    isValid = !wallMap[x][y].IsFilled();
//                    spawnAttempt++;
//                } while (!isValid && spawnAttempt < enemySpawnAttempts);

//                GameObject enemyToSpawn = enemyTiles[GameManager.instance.Rnd.Next(enemyTiles.Length)];
//                GameObject enemy = Instantiate(enemyToSpawn, new Vector3(x, y, 0f), Quaternion.identity);
//                enemy.transform.parent = enemyContainer;
//            }
//        }

//        void PlaceEntryAndExit()
//        {
//            int attempt = 0;
//            bool validExit = false;
//            while (!validExit || attempt < exitSpawnAttempts)
//            {
//                PlaceEntrance();
//                validExit = PlaceExit();
//                attempt++;
//            }
//            if (!validExit)
//                throw new System.Exception("Could not find valid exit.");
//        }

//        void PlaceEntrance()
//        {
//            // Delete any existing entry.
//            if (entryTile != null)
//            {
//                entryTile.cell.RemoveTile(entryTile);
//                Destroy(entryTile.gameObject);
//                entryTile = null;
//            }

//            // Get random zone.
//            int entryZoneIndex = GameManager.instance.Rnd.Next(zones.Length);
//            TileZone entryZone = zones[entryZoneIndex];

//            // Place entrance in random cell of random zone.
//            int entryCellIndex = GameManager.instance.Rnd.Next(entryZone.Count);
//            Cell entryCell = entryZone[entryCellIndex];

//            entryTile = Instantiate(entryTilePrefab).GetComponent<SpawnTile>();
//            entryCell.AddTile(entryTile, true);

//            Debug.Log("Entry is located at Cell (" + entryCell.x + ", " + entryCell.y + ")");
//        }

//        bool PlaceExit()
//        {
//            // Use breadth-first search to get all cells whose distance to the entrance is within
//            // the exitDistanceRange and is within a zone.
//            /*Queue<PathStep<Cell>> openList = new Queue<PathStep<Cell>>();
//            bool[][] visited = new bool[cols][];
//            for (int i = 0; i < cols; ++i)
//                visited[i] = new bool[rows];

//            openList.Enqueue(new PathStep<Cell>(entryCell));

//            List<Cell> cellsInRange = new List<Cell>();

//            while (openList.Count > 0)
//            {
//                // Pop the next step in openList and mark visited.
//                PathStep<Cell> curStep = openList.Dequeue();
//                Cell curCell = curStep.data;
//                visited[curCell.x][curCell.y] = true;


//                // Add curCell to cellsInRange if it is in range and is zoned.
//                if (exitDistanceRange.InRange(curStep.gCost) && curCell.zoneNumber >= 0)
//                    cellsInRange.Add(curCell);

//                // Don't proceed if this cell's distance is at max.
//                if (curStep.gCost >= exitDistanceRange.max)
//                    continue;

//                List<Cell> adjacentCells = GetAdjacentCells(curCell);
//                foreach (Cell adjCell in adjacentCells)
//                {
//                    PathStep<Cell> adjCellStep = new PathStep<Cell>(adjCell);
//                    if (!adjCell.IsBlocking && !visited[adjCell.x][adjCell.y])
//                    {
//                        adjCellStep.gCost = curStep.gCost + 1;
//                        adjCellStep.parent = curStep;
//                        openList.Enqueue(adjCellStep);
//                    }
//                }
//            }

//            // Place exit in random cell of cellsInRange.
//            int exitCellIndex = GameManager.instance.Rnd.Next(cellsInRange.Count);
//            Cell exitCell = cellsInRange[exitCellIndex];*/

//            if (entryTile.cell == null)
//                throw new System.Exception("Entry tile must be instantiated.");

//            if (exitTile != null)
//            {
//                exitTile.cell.RemoveTile(exitTile);
//                Destroy(exitTile.gameObject);
//                exitTile = null;
//            }

//            // Loop through each zone, searching for a valid point.
//            IntRange sqrExitDistanceRange;
//            sqrExitDistanceRange.min = (int)Mathf.Pow(exitDistanceRange.min, 2);
//            sqrExitDistanceRange.max = (int)Mathf.Pow(exitDistanceRange.max, 2);

//            int exitZoneIndex = 0;
//            TileZone exitZone = zones[exitZoneIndex];

//            Cell exitCell = null;
//            while (exitCell == null && exitZoneIndex < zones.Length)
//            {
//                for (int i = 0; i < exitZone.Count; ++i)
//                {
//                    float sqrDistance = Mathf.Pow(exitZone[i].x - entryTile.cell.x, 2) + 
//                                             Mathf.Pow(exitZone[i].y - entryTile.cell.y, 2);
//                    if (sqrExitDistanceRange.IsInRange(sqrDistance))
//                    {
//                        exitCell = exitZone[i];
//                        break;
//                    }
//                }
//                exitZoneIndex++;
//            }

//            if (exitCell == null)
//                return false;

//            exitTile = Instantiate(exitTilePrefab).GetComponent<ExitTile>();
//            exitCell.AddTile(exitTile, true);

//            Debug.Log("Exit is located at Cell (" + exitCell.x + ", " + exitCell.y + ")");

//            return true;
//        }

//        void PlaceFood()
//        {
//            int foodCount = GameManager.instance.Rnd.Next(foodRange.min, foodRange.max + 1);
//            int foodSpawnCycles = 0;

//            while (foodCount > 0 && foodSpawnCycles < foodSpawnCycleAttempts)
//            {
//                int zoneIndex = GameManager.instance.Rnd.Next(zones.Length);
//                TileZone zone = zones[zoneIndex];

//                int foodIndex = GameManager.instance.Rnd.Next(foodTiles.Length);
//                GameObject food = foodTiles[foodIndex];

//                int minAdjBlocking = 3;
//                bool valid = false;
//                while (!valid && minAdjBlocking >= 0)
//                {
//                    valid = PlaceTileInZone(zone, food, minAdjBlocking);
//                    minAdjBlocking--;
//                }
//                if (valid)
//                    foodCount--;

//                foodSpawnCycles++;
//            }
//            if (foodCount != 0)
//                throw new System.Exception("Couldnt place food");
//        }

//        bool PlaceTileInZone(TileZone zone, GameObject tile, int minAdjBlocking = 0)
//        {
//            // Try to put food in cell surrounded by 3 blocking cells.
//            int attempts = 0;
//            while (attempts < foodSpawnAttempts)
//            {
//                int cellIndex = GameManager.instance.Rnd.Next(zone.Count);
//                Cell cell = zone[cellIndex];

//                bool placeTile = false;
//                if (minAdjBlocking > 0)
//                {
//                    // Get number of adjacent cells that are blocking cells
//                    int filledCount = 0;
//                    List<Cell> adjCells = GetAdjacentCells(cell);
//                    foreach (Cell c in adjCells)
//                    {
//                        if (c.IsBlocking)
//                            filledCount++;
//                    }
//                    if (filledCount >= minAdjBlocking)
//                        placeTile = true;
//                }
//                else
//                    placeTile = true;

//                if (placeTile)
//                {
//                    GameObject tileInstance = Instantiate(tile);
//                    cell.AddTile(tileInstance);
//                    Debug.Log("Placed food in " + cell);
//                    return true;
//                }

//                attempts++;
//            }

//            return false;
//        }

//        /* Return a list with the Von Neumann adjacent cells.
//         */
//        List<Cell> GetAdjacentCells(Cell cell)
//        {
//            List<Cell> adjCells = new List<Cell>();

//            // Left
//            if (IsPointInBounds(cell.x - 1, cell.y))
//                adjCells.Add(cells[cell.x - 1][cell.y]);

//            // Right
//            if (IsPointInBounds(cell.x + 1, cell.y))
//                adjCells.Add(cells[cell.x + 1][cell.y]);

//            // Up
//            if (IsPointInBounds(cell.x, cell.y + 1))
//                adjCells.Add(cells[cell.x][cell.y + 1]);

//            // Down
//            if (IsPointInBounds(cell.x, cell.y - 1))
//                adjCells.Add(cells[cell.x][cell.y - 1]);

//            return adjCells;
//        }

//        public GameObject SpawnPlayerAtEntry()
//        {
//            return entryTile.Spawn();
//        }

//        public bool IsPointInBounds(int x, int y)
//        {
//            return (x >= 0 && x < cols && y >= 0 && y < rows);
//        }
//    }
//}
