using CellAuto;
using UnityEngine;

public class BoardManager : MonoBehaviour 
{
    public int maxEdgeSize;
    public float maxAspectRatio;

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

    Transform boardContainer;      // Container for board tiles.
    Transform enemyContainer;

    public Automator wallMap;

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

        // Surround board with outer wall tiles and fill with floor tiles.
        for (int x = -1; x < cols + 1; ++x)
        {
            for (int y = -1; y < rows + 1; ++y)
            {
                GameObject[] tileSet = floorTiles;

                // If the tile is an edge tile, use the outer wall tile set.
                if (x == -1 || x == cols || y == -1 || y == rows)
                    tileSet = outerWallTiles;

                GameObject toInstantiate = tileSet[GameManager.instance.Rnd.Next(tileSet.Length)];

                GameObject instance = 
                    Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity);
                
                instance.transform.parent = boardContainer;

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
                    GameObject wallInstance = Instantiate(wallToInstatiate, new Vector3(x, y, 0f), Quaternion.identity);
                    wallInstance.transform.parent = boardContainer;
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
