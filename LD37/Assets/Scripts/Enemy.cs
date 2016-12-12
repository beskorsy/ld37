using UnityEngine;
using System.Collections;

public class Enemy : MovingObject
{
    public int hp = 100;
    public float timeBetweenSlash;

    private Transform target;
    private SpriteRenderer spriteRenderer;
    private float timer;
    private Animator anim;
    private bool isDead;


    // Use this for initialization
    void Awake()
    {
        anim = GetComponent<Animator>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        float xDir = 0;
        float yDir = 0;

        if (target != null && !isDead)
        {
            if (Mathf.Abs(target.position.y - transform.position.y) > 0.3f)
                yDir = target.position.y > transform.position.y ? 1 : -1;

            if (Mathf.Abs(target.position.x - transform.position.x) > 0.3f)
                xDir = target.position.x > transform.position.x ? 1 : -1;

            /**
            boxCollider.enabled = false;
            RaycastHit2D hit = Physics2D.Linecast(transform.position, transform.position + new Vector3(0.1f, 0.1f, 0), blockingLayer);
            boxCollider.enabled = true;
            if (hit.transform != null)
            {
                r = true;
                xDir = - 5;
                yDir =  -5;
            }
    **/

            if (xDir != 0 || yDir != 0) {
                Move2(new Vector2(xDir, yDir).normalized);
            }

            timer += Time.fixedDeltaTime;

            if (timer >= timeBetweenSlash)
                TrySlash();
        }
    }

    public void Damage(int value)
    {
        if (isDead) return;

        hp -= value;

        if (hp <= 0)
        {
            isDead = true;
            Dead();
            GameManager.instance.OnEnemyDestroy();
        }
    }

    private void Dead()
    {
        anim.SetTrigger("Dead");
        collider.enabled = false;
    }

    private void TrySlash()
    {
        if (Slash.TryActionEnemy(transform.position, 0.15f, 10))
        {
            timer = 0f;
        }
    }
}
