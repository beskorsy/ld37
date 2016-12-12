using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MovingObject
{
    public int hp = 100;
    public float timeBetweenSlash;
    public GameObject blood;
    public RuntimeAnimatorController[] animControllers;

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
        int index = Random.Range(0, animControllers.Length);
        anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = animControllers[index];
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
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
            float deltaY = Mathf.Abs(target.position.y - transform.position.y);
            float deltaX = Mathf.Abs(target.position.x - transform.position.x);

            if (deltaY > 2.0f || deltaX > 2.0f)
            {
                List<GameObject> epList = GameManager.instance.GetEnemyPointList();
                List<Vector2> deltas = new List<Vector2>();
                foreach (GameObject ep in epList)
                {
                    float epDeltaY = Mathf.Abs(ep.transform.position.y - transform.position.y);
                    float epDeltaX = Mathf.Abs(ep.transform.position.x - transform.position.x);

                    deltas.Add(new Vector2(epDeltaX, epDeltaY));
                }

                Vector2 epVector = new Vector2(0, 0);
                float min = 100000f;

                foreach (Vector2 v in deltas)
                {
                    if (v.magnitude < min)
                    {
                        min = v.magnitude;
                        epVector = v;
                    }
                }

                deltaY = Mathf.Abs(epVector.y);
                deltaX = Mathf.Abs(epVector.x);
            }
                        

           if (deltaY > 0.2f)
                yDir = target.position.y > transform.position.y ? 1 : -1;

            if (deltaX > 0.2f)
                xDir = target.position.x > transform.position.x ? 1 : -1;

            if (xDir != 0 || yDir != 0)
            {
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
