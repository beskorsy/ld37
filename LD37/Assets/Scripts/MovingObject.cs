using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour {
    public float speed = 6f;
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private Vector3 movement;

    // Use this for initialization
    protected virtual void Start () {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
    }
	
	 protected void Move(float h, float v)
    {
        if (h == 0 && v == 0)
            return;

        movement.Set(h, v, 0f);
        
        movement = movement.normalized * speed * Time.deltaTime;

        boxCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(transform.position, transform.position + movement, blockingLayer);

        boxCollider.enabled = true;

        if (hit.transform == null)
            rb2D.MovePosition(transform.position + movement);
    }
}
