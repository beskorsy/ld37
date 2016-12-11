using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MovingObject {

    public float timeBetweenSlash = 0.25f;
    public Sprite attackSprite;
    public Sprite sprite;
    public int hp = 100;
    public Text hpText;
    public Image damageImage;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    private float timer;
    private SpriteRenderer spriteRenderer;
    private bool damaged;


    void Awake () {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;

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

        hpText.text = "HP: " + hp;

        damaged = true;

        if (hp <= 0)
        {
            Destroy(gameObject);
            GameManager.instance.enemiesCount--;
        }
    }
}
