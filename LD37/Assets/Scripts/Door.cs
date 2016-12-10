using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    public float closeTime = 1f;

    private BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Start () {
        GameManager.instance.AddDoorToList(this);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
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
