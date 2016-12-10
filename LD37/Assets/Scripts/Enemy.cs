using UnityEngine;
using System.Collections;

public class Enemy : MovingObject
{

    private Rigidbody2D rb2d;
    private Transform target;

    // Use this for initialization
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float xDir = 0;
        float yDir = 0;

        yDir = target.position.y > transform.position.y ? 1 : -1;
        xDir = target.position.x > transform.position.x ? 1 : -1;

        Move(xDir, yDir);
    }
}
