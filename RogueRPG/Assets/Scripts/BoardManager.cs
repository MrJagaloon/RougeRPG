using UnityEngine;

namespace Board
{
    public class BoardManager : MonoBehaviour
    {
        public int maxEdgeSize;
        public float maxAspectRatio;
        public int edgeSize;

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
            for (int x = 0; x < cols; ++x)
            {
                cells[x] = new Cell[rows];
                for (int y = 0; y < rows; ++y)
                {
                    // Instatiate random floor tile.
                    int floorIndex = GameManager.instance.Rnd.Next(floorTiles.Length);
                    GameObject floorToInstantiate = floorTiles[floorIndex];
                    GameObject floorInstance =
                        Instantiate(floorToInstantiate, new Vector3(x, y, 0f), Quaternion.identity);

                    // Instantiate cell.
                    Cell cell = Cell.NewCell(x, y);
                    cell.transform.parent = boardContainer;
                    cell.AddTile(floorInstance, true);
                    cells[x][y] = cell;
                }
            }

            // Fill boundaries of map

            // Middle
            for (int x = 0; x < cols; ++x)
            {
                // Bottom
                for (int y = -edgeSize; y < 0; ++y)
                {
                    float chance = 1 / ((edgeSize - -y) / (edgeSize + 1));
                    bool doSpawn = GameManager.instance.Rnd.Next(100) < chance;
                    if (doSpawn)
                    {
                        GameObject toInstantiate = 
                            outerWallTiles[GameManager.instance.Rnd.Next(outerWallTiles.Length)];
                        Instantiate(toInstantiate);
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
    }
}
