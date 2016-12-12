using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MovingObject
{

    public float timeBetweenSlash;
    public int hp = 100;
    public Text hpText;
    public Image damageImage;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    [HideInInspector]
    public bool isDead;

    private float timer = 0f;
    private bool damaged;
    private AudioSource drinkAudio;
    private AudioSource slashAudio;
    private AudioSource wilhelmAudio;
    private Animator anim;

    private Vector2 input = Vector2.zero;

    public Vector2 inputVector { get { return input; } }

    void Awake()
    {
        anim = GetComponent<Animator>();

        AudioSource[] sounds = GetComponents<AudioSource>();
        drinkAudio = sounds[0];
        slashAudio = sounds[1];
        wilhelmAudio = sounds[2];
        timer = timeBetweenSlash;
    }

    private void Update()
    {
        if (isDead || GameManager.instance.isDlgShow) return;

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
        if (Input.GetKey(KeyCode.Space) && timer >= 0.4f)
            PlayerSlash();

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        anim.SetBool("Walk", !Mathf.Approximately(0, h) || !Mathf.Approximately(0, v));
        input.x = h;
        input.y = v;
    }

    void FixedUpdate()
    {
        if (isDead || Mathf.Approximately(input.magnitude, 0) || GameManager.instance.isDlgShow) return;
        Move2(input.normalized);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Health"))
        {
            Destroy(collision.gameObject);
            RestorHP(10);
        }
    }

    private void PlayerSlash()
    {
        timer = 0f;
        anim.SetTrigger("Slash");
        slashAudio.Play();

        Slash.ActionPlayer(transform.position, 0.3f, 50, true);
    }

    public void Damage(int value)
    {
        hp -= value;

        hpText.text = "HP: " + hp;

        damaged = true;

        if (hp <= 0)
        {
            isDead = true;
            anim.SetTrigger("Dead");
            wilhelmAudio.Play();

            Invoke("GameOver", 3f);
        }
    }

    public void RestorHP(int value)
    {
        hp += value;
        drinkAudio.Play();

        hpText.text = "HP: " + hp;
    }

    private void GameOver()
    {
        GameManager.instance.GameOverDlg();
    }
}
