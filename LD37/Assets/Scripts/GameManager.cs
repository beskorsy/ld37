using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public float levelStartDelay = 2f;
    public static GameManager instance = null;
    public GameObject enemy;
    public float spawnTime = 2f;
    [HideInInspector]
    public int enemiesCount;

    private Text scoreText;
    private Text levelText;
    private GameObject levelImage;
    private List<Door> doors;
    private Player player;
    private int level = 0;
    private int score;
    private int maxEnemy;
    private bool doingSetup;


    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        doors = new List<Door>();
        
        player = GameObject.FindGameObjectWithTag("Player").GetComponents<Player>()[0];
    }

    private void Start()
    {
        Invoke("MenuOut", 3f);
    }

    void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    private void Update()
    {

    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        level++;
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
        doingSetup = false; 

       // levelImage = GameObject.Find("LevelImage");
        //levelText = GameObject.Find("LevelText").GetComponent<Text>();
       // if (level > 1)
        //    levelText.text = "Room " + level; 
       // levelImage.SetActive(true);
       // Invoke("HideLevelImage", levelStartDelay);

        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        maxEnemy = level * 2 + 1;
        InvokeRepeating("TrySpawnEnemy", 0f, spawnTime);
        doors.Clear();
    }

    public void AddDoorToList(Door script)
    {
        doors.Add(script);
    }

    public void OnOpenDoorOut(Door scrip)
    {
        int doorIndex = Random.Range(0, doors.Count);
        Door nextDoor = doors[doorIndex];
        nextDoor.OnOpenDoorIn();
        Vector2 nextDoorPos = nextDoor.transform.position;

        player.gameObject.transform.position = nextDoorPos;
    }

    private void TrySpawnEnemy()
    {
        if (enemiesCount <= maxEnemy && !doingSetup)
        {
            int doorIndex = Random.Range(0, doors.Count);
            Door spawnDoor = doors[doorIndex];
            Vector2 spawnDoorPos = spawnDoor.transform.position;
            enemiesCount++;

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
