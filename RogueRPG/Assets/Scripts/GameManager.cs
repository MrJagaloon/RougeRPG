using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int level;
    public int difficulty;
    public string seedString;
    public float levelStartDelay = 2f;
    public float nextLevelDelay = 1f;
    public float turnDelay = 0.1f;

    public int playerHealth = 100;
    public GameObject playerPrefab;
    public GameObject playerInstance;
    public Player playerScript;

    public float camSize = 5f;
    public IntRange camSizeRange;
    public float zoomSpeed = 1f;

    System.Random rnd;
    public System.Random Rnd { get { return rnd; } }

    [HideInInspector] public bool playersTurn = true;

    Text levelText;
    GameObject levelImage;
    bool doingSetup;

    List<Enemy> enemies;
    bool enemiesMoving;
    public int EnemyCount { get { return enemies.Count; } }

    Board.BoardManager boardManager;

    int seed;


    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        boardManager = GetComponent<Board.BoardManager>();

        if (string.IsNullOrEmpty(seedString))
            seedString = System.DateTime.UtcNow.ToString();
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        instance.level++;
        instance.InitGame();
    }
    void OnEnable() { SceneManager.sceneLoaded += OnLevelFinishedLoading; }
    void OnDisable() { SceneManager.sceneLoaded -= OnLevelFinishedLoading; }

    void InitGame()
    {
        doingSetup = true;

        // Update the scene
        seed = (seedString + level).GetHashCode();
        rnd = new Random(seed);

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);

        enemies.Clear();

        boardManager.SetupScene(level);

        playerInstance = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        playerScript = playerInstance.GetComponent<Player>();

        Camera.main.orthographicSize = camSize;
    }

    void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void EndLevel()
    {
        Invoke("NextLevel", nextLevelDelay);
    }

    void NextLevel()
    {
        SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        levelText.text = "After " + level + " days, you died.";
        levelImage.SetActive(true);
        enabled = false;
    }

    void Update()
    {
        // Zoom by changing the camera's orthoganal size.
        float zoom = Input.GetAxis("Zoom");
        camSize -= zoom * zoomSpeed * Time.deltaTime;
        camSize = Mathf.Clamp(camSize, camSizeRange.min, camSizeRange.max);
        Camera.main.orthographicSize = camSize;

        if (playersTurn || enemiesMoving || doingSetup)
            return;

        StartCoroutine(MoveEnemies());
	}

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;

        for (int i = 0; i < enemies.Count; ++i)
        {
            enemies[i].MoveEnemy();

            //yield return new WaitForSeconds(enemies[i].moveTime);
        }

        yield return new WaitForSeconds(turnDelay);

        enemiesMoving = false;
        playersTurn = true;
    }
}
