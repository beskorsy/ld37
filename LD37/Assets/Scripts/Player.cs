using UnityEngine;
using System.Collections;

public class Player : MovingObject {

    public float timeBetweenSlash = 0.25f;
    public Sprite attackSprite;
    public Sprite sprite;
    public int hp = 100;

    private float timer;
    private SpriteRenderer spriteRenderer;


    void Awake () {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && timer >= timeBetweenSlash)
            PlayerSlash();
    }

    void FixedUpdate  () {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Door"))
        {
            
        }
    }

    private void PlayerSlash()
    {
        timer = 0f;
        spriteRenderer.sprite = attackSprite;
        Invoke("SlashOut", 0.1f);

        Slash.ActionPlayer(transform.position, 0.4f, 50, true);

       // if (gameObject.CompareTag(""))
    }

    private void SlashOut()
    {
        spriteRenderer.sprite = sprite;
    }

    public void Damage(int value)
    {
        hp -= value;

        if (hp <= 0)
        {
            Destroy(gameObject);
            GameManager.instance.enemiesCount--;
        }
    }
}
