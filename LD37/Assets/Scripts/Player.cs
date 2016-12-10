using UnityEngine;
using System.Collections;

public class Player : MovingObject {
    
    void Awake () {
    }
	
	// Update is called once per frame
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
}
