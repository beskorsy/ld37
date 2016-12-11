using UnityEngine;
using System.Collections;

public class Enemy : MovingObject
{
    public int hp = 100;
    public float timeBetweenSlash = 0.6f;
    public Sprite attackSprite;
    public Sprite sprite;

    private Transform target;
    private SpriteRenderer spriteRenderer;
    private float timer;

    // Use this for initialization
    void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float xDir = 0;
        float yDir = 0;

        if (target != null)
        {
            if (Mathf.Abs(target.position.y - transform.position.y) > 0.3f)
                yDir = target.position.y > transform.position.y ? 1 : -1;

            if (Mathf.Abs(target.position.x - transform.position.x) > 0.3f)
                xDir = target.position.x > transform.position.x ? 1 : -1;

            if (xDir != 0 || yDir != 0)
                Move(xDir, yDir);

            timer += Time.deltaTime;
            if (timer >= timeBetweenSlash)
                TrySlash();
        }
    }

    public void Damage(int value)
    {
        hp -= value;

        if (hp <= 0)
        {
            Destroy(gameObject);
            GameManager.instance.OnEnemyDestroy();
        }
    }

    private void TrySlash()
    {
        if (Slash.TryActionEnemy(transform.position, 0.3f, 10))
        {
            timer = 0f;
            spriteRenderer.sprite = attackSprite;
            Invoke("SlashOut", 0.1f);
        }
    }
}
