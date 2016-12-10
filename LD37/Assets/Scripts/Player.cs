using UnityEngine;
using System.Collections;

public class Player : MovingObject {

    private Rigidbody2D rb2d;

    // Use this for initialization
    void Awake () {
        rb2d = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void FixedUpdate  () {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        RaycastHit2D hit;
        // Move the player around the scene.
        Move(h, v);

    }
}
