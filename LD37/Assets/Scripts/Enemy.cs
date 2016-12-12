using UnityEngine;
using System.Collections;

public class Enemy : MovingObject
{
    public int hp = 100;
    public float timeBetweenSlash;
    public GameObject blood;

    private Transform target;
    private float timer;
    private Animator anim;
    private Animator bloodAnim;
    private bool isDead;
    private AudioSource hitAudio;
    private AudioSource deadAudio;

    // Use this for initialization
    void Awake()
    {
        anim = GetComponent<Animator>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        bloodAnim = blood.GetComponent<Animator>();

        AudioSource[] sounds = GetComponents<AudioSource>();
        hitAudio = sounds[0];
        deadAudio = sounds[1];
    }

    void FixedUpdate()
    {
        float xDir = 0;
        float yDir = 0;

        if (target != null && !isDead && !GameManager.instance.isDlgShow)
        {
            if (Mathf.Abs(target.position.y - transform.position.y) > 0.2f)
                yDir = target.position.y > transform.position.y ? 1 : -1;

            if (Mathf.Abs(target.position.x - transform.position.x) > 0.2f)
                xDir = target.position.x > transform.position.x ? 1 : -1;
            
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
        bloodAnim.SetTrigger("Hit");
        if (isDead) return;

        hp -= value;


        if (hp <= 0)
        {
            deadAudio.Play();
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
        if (Slash.TryActionEnemy(transform.position, 0.20f, 10))
        {
            hitAudio.Play();
            timer = 0f;
        }
    }
}
