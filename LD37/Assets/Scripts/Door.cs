using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{

    public float closeTime = 1f;

    private BoxCollider2D boxCollider;
    private bool needOpen;
    private AudioSource doorAudio;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        doorAudio = GetComponent<AudioSource>();
    }

    void Start()
    {
        GameManager.instance.AddDoorToList(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !needOpen)
        {
            needOpen = true;
            Invoke("OnOpenDoorOut", 0.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
            needOpen = false;   
    }


    private void OnOpenDoorOut()
    {
        if (needOpen)
        {
            doorAudio.Play();
            GameManager.instance.OnOpenDoorOut(this);
        }
    }

    public void OnOpenDoorIn()
    {
        boxCollider.enabled = false;
        Invoke("Ready", closeTime);
    }

    void Ready()
    {
        boxCollider.enabled = true;
    }
}
