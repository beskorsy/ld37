using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public float levelStartDelay = 2f;
    public static GameManager instance = null;
    public GameObject[] enemys;
    public float spawnTime = 2f;

    [HideInInspector]
    public int enemiesCount;
    [HideInInspector]
    public bool isDlgShow;

    private Text scoreText;
    private Text levelText;
    private Text keysText;
    private Text scoreMenuText;
    private GameObject levelImage;
    private List<Door> doors;
    private Player player;
    private int level = 1;
    private int score;
    private int maxEnemy;
    private bool readyToRestart;


    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        doors = new List<Door>();
    }

    private void Update()
    {
        if (readyToRestart)
        {
            if (Input.GetKey(KeyCode.R))
            {
                Restart();
            }
        }
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        InitGame();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void InitGame()
    {
        isDlgShow = true;

        InitUI();

        player = GameObject.FindGameObjectWithTag("Player").GetComponents<Player>()[0];

        enemiesCount = 0;
        maxEnemy = level * 3 + 1;
        InvokeRepeating("TrySpawnEnemy", 0f, spawnTime);
        doors.Clear();
    }

    public void InitUI()
    {
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        keysText = GameObject.Find("KeysText").GetComponent<Text>();
        scoreMenuText = GameObject.Find("ScoreMenuText").GetComponent<Text>();
        if (level > 1)
        {
            levelText.text = "Room " + level;
        }
        keysText.text = "WASD - Move\n SPASE - Hit";
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);

        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        scoreText.text = "Score: " + score;
    }

    void HideLevelImage()
    {
        levelImage.SetActive(false);
        isDlgShow = false;
    }

    public void AddDoorToList(Door script)
    {
        doors.Add(script);
    }

    public void OnOpenDoorOut(Door script)
    {
        if (player.isDead) return;

        if (TryNextLevel()) return;

        doors.Remove(script);

        int doorIndex = Random.Range(0, doors.Count);
        Door nextDoor = doors[doorIndex];
        Vector2 nextDoorPos = nextDoor.transform.position;

        doors.Remove(nextDoor);
        Destroy(script.gameObject);
        Destroy(nextDoor.gameObject);

        player.gameObject.transform.position = nextDoorPos;
    }

    private bool TryNextLevel()
    {
        if (doors.Count <= 2)
        {
            OpenNextLevel();
            return true;
        } else
        {
            float r = Random.Range(0, doors.Count);
            if (r <= 2)
            {
                OpenNextLevel();
                return true;
            }
        }
        return false;
    }

    private void OpenNextLevel()
    {
        if (level >= 2)
        {
            WinDlg();
        }
        else
        {
            level++;
            SceneManager.LoadScene("Level" + level);
        }
    }

    private void WinDlg()
    {
        isDlgShow = true;
        levelText.text = "Win!";
        scoreMenuText.text = "Score: " + score;
        keysText.text = "R - Restart";

        levelImage.SetActive(true);
        readyToRestart = true;
    }

    public void GameOverDlg()
    {
        isDlgShow = true;
        levelText.text = "Game Over!";
        scoreMenuText.text = "Score: " + score;
        keysText.text = "R - Restart";

        levelImage.SetActive(true);
        readyToRestart = true;
    }

    private void Restart()
    {
        score = 0;
        level = 1;
        SceneManager.LoadScene("Level1");
    }

    private void TrySpawnEnemy()
    {
        if (enemiesCount <= maxEnemy && !isDlgShow)
        {
            int doorIndex = Random.Range(0, doors.Count);
            Door spawnDoor = doors[doorIndex];
            Vector2 spawnDoorPos = spawnDoor.transform.position;
            enemiesCount++;

            GameObject enemy = enemys[Random.Range(0, enemys.Length)];
            Instantiate(enemy, spawnDoorPos, Quaternion.identity);
        }
    }

    public void OnEnemyDestroy()
    {
        enemiesCount--;
        score += 10;
        scoreText.text = "Score: " + score;
    }
}
