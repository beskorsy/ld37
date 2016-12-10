﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;
    public GameObject enemy;
    private int maxEnemy;
    public float spawnTime = 2f;

    private List<Door> doors;
    public int enemiesCount;
    private Player player;
    private int level = 1;


    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        doors = new List<Door>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponents<Player>()[0];
        InitGame();
    }

    private void Update()
    {

    }

   // void OnLevelWasLoaded(int index)
    //{
      //  level++;
       // InitGame();
    //}


    void InitGame()
    {
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
        if (enemiesCount <= maxEnemy)
        {
            int doorIndex = Random.Range(0, doors.Count);
            Door spawnDoor = doors[doorIndex];
            Vector2 spawnDoorPos = spawnDoor.transform.position;
            enemiesCount++;

            Instantiate(enemy, spawnDoorPos, Quaternion.identity);
        }
    }
}
