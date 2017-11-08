using UnityEngine;

namespace Board
{
    public class BoardManager : MonoBehaviour
    {
        public int maxEdgeSize;
        public float maxAspectRatio;
        public int edgeSize;

        public float edgeChanceMod;

        int cols;
        int rows;

        public int enemySpawnAttempts = 100;
        //public Count foodCount = new Count(1, 4);

        public GameObject[] exitTiles;
        public GameObject[] floorTiles;
        public GameObject[] wallTiles;
        public GameObject[] outerWallTiles;
        public GameObject[] foodTiles;
        public GameObject[] enemyTiles;

        public CellAuto.Automator wallMap;

        Transform boardContainer;      // Container for board tiles.
        Transform enemyContainer;

        Cell[][] cells;

        /* Surround the board with random outerWallTiles and fill with random floorTiles.
         */
        void InitBoard()
        {
            boardContainer = new GameObject("Board").transform;
            enemyContainer = new GameObject("Enemies").transform;

            // Intantiate an empty to contain the outer wall tiles
            Transform outerWallTilesContainer = new GameObject("Outer Wall Tiles").transform;
            outerWallTilesContainer.transform.parent = boardContainer;

            // Get random size 2202.9.
            float aspectRatio = (int)GameManager.instance.Rnd.Next(100, (int)maxAspectRatio * 100) / 100f;
            bool xIsDominant = GameManager.instance.Rnd.Next(0, 100) > 50;
            if (xIsDominant)
            {
                cols = maxEdgeSize;
                rows = (int)(cols / aspectRatio);
            }
            else
            {
                // a = c / r
                // c = a * r
                rows = maxEdgeSize;
                cols = (int)(aspectRatio * rows);
            }

            Debug.Log("Generating board with\n    Aspect Ratio: " + aspectRatio + "\n    Rows: " + rows + "\n    Cols: " + cols);

            // Initialize cells with random floor tiles.
            cells = new Cell[cols][];
            for (int x = -edgeSize; x < cols + edgeSize; ++x)
            {
                if (IsPointInBounds(x, 0))
                    cells[x] = new Cell[rows];
                for (int y = -edgeSize; y < rows + edgeSize; ++y)
                {
                    if (IsPointInBounds(x, y))
                    {
                        // Point is in bounds, so fill it with cells with random floor tiles.

                        // Instatiate random floor tile.
                        int floorIndex = GameManager.instance.Rnd.Next(floorTiles.Length);
                        GameObject floorInstance =
                            Instantiate(floorTiles[floorIndex], new Vector3(x, y, 0f), Quaternion.identity);

                        // Instantiate cell.
                        Cell cell = Cell.NewCell(x, y);
                        cell.transform.parent = boardContainer;
                        cell.AddTile(floorInstance, true);
                        cells[x][y] = cell;
                    }
                    else
                    {
                        // Point is out of bounds, so fill it with outer wall tiles.
                        // Spawn the outer walls randomly with a chance proportional to the distance 
                        // to the nearest edge of the board. 

                        float distance = 0f;

                        if (x < 0 && y < 0)                 // Bottom left
                            distance = Mathf.Sqrt(Mathf.Pow(-x - 1, 2) + Mathf.Pow(-y - 1, 2));
                        else if (x < 0 && y >= rows)        // Top left
                            distance = Mathf.Sqrt(Mathf.Pow(-x - 1, 2) + Mathf.Pow(y - rows, 2));
                        else if (x < 0)                     // Left
                            distance = -x - 1;
                        else if (x >= cols && y < 0)        // Bottom right
                            distance = Mathf.Sqrt(Mathf.Pow(x - cols, 2) + Mathf.Pow(-y - 1, 2));
                        else if (x >= cols && y >= rows)     // Top right
                            distance = Mathf.Sqrt(Mathf.Pow(x - cols, 2) + Mathf.Pow(y - rows, 2));
                        else if (x >= cols)                 // Right
                            distance = x - cols;
                        else if (y < 0)                     // Bottom
                            distance = -y - 1;
                        else if (y >= rows)                 // Top
                            distance = y - rows;

                        float chance = (1f - distance / edgeSize) * edgeChanceMod; 

                        if (GameManager.instance.Rnd.Next(100) < chance * 100f)
                        {
                            int wallIndex = GameManager.instance.Rnd.Next(outerWallTiles.Length);
                            GameObject wallInstance =
                                Instantiate(outerWallTiles[wallIndex], new Vector3(x, y, 0f), Quaternion.identity);
                            wallInstance.transform.parent = outerWallTilesContainer;
                        }
                    }
                }
            }
        }

        /* Initalize the board and fill with procedurally generated walls, enemies, and others.
         */
        public void SetupScene(int level)
        {
            InitBoard();

            wallMap.Generate(rows, cols, GameManager.instance.Rnd);

            /* Loop through each cell of the wallMap. If it is "alive", place a random wall at that 
             * cell's position on the board.
             */
            for (int x = 0; x < cols; ++x)
            {
                for (int y = 0; y < rows; ++y)
                {
                    if (wallMap[x][y].IsFilled())
                    {
                        GameObject wallToInstatiate = wallTiles[GameManager.instance.Rnd.Next(wallTiles.Length)];
                        GameObject wallInstance = Instantiate(wallToInstatiate, Vector3.zero, Quaternion.identity);
                        wallInstance.transform.parent = boardContainer;
                        cells[x][y].AddTile(wallInstance, true);
                    }
                }
            }

            // Generate enemies.
            int enemyCount = (int)Mathf.Log(level, 2) * GameManager.instance.difficulty;
            for (int i = 0; i < enemyCount; ++i)
            {
                int x, y;
                int spawnAttempt = 0;
                bool isValid = false;

                do
                {
                    x = GameManager.instance.Rnd.Next(cols);
                    y = GameManager.instance.Rnd.Next(rows);
                    isValid = !wallMap[x][y].IsFilled();
                    spawnAttempt++;
                } while (!isValid && spawnAttempt < enemySpawnAttempts);

                GameObject enemyToSpawn = enemyTiles[GameManager.instance.Rnd.Next(enemyTiles.Length)];
                GameObject enemy = Instantiate(enemyToSpawn, new Vector3(x, y, 0f), Quaternion.identity);
                enemy.transform.parent = enemyContainer;
            }

            // Instatiate exit tile in top right corner of board.
            Instantiate(exitTiles[GameManager.instance.Rnd.Next(exitTiles.Length)], new Vector3(cols - 1, rows - 1, 0f), Quaternion.identity);
        }

        public bool IsPointInBounds(int x, int y)
        {
            return (x >= 0 && x < cols && y >= 0 && y < rows);
        }
    }
}
