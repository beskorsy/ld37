using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    public float speed = 6f;
    public LayerMask blockingLayer;

    protected Collider2D collider;
    private Rigidbody2D rb2D;
    private Vector3 movement;

    // Use this for initialization
    protected virtual void Start()
    {
        collider = GetComponent<Collider2D>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    protected void Move(float h, float v)
    {
        if (h == 0 && v == 0)
            return;

        movement.Set(h, v, 0f);

        movement = movement.normalized * speed * Time.deltaTime;

        collider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(transform.position, transform.position + movement, blockingLayer);

        collider.enabled = true;

        if (hit.transform == null) {
            Vector3 diff = movement;

            diff.Normalize();

            float rot_z = (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);

            Quaternion newRot = Quaternion.Euler(new Vector3(0, 0, rot_z + 90));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, Time.deltaTime * 1500f);

            rb2D.MovePosition(transform.position + movement);
        }
    }

    protected void Move2(Vector2 movement, float dt)
    {
        rb2D.AddForce(movement * speed * dt, ForceMode2D.Force);
        //Debug.Log("move2 by " + (movement * speed * dt).ToString());

        float rot_z = (Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg);
        Quaternion newRot = Quaternion.Euler(new Vector3(0, 0, rot_z + 90));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, Time.deltaTime * 1500f);
    }
}
