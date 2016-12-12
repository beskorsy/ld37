using UnityEngine;

public class Door : MonoBehaviour
{
    private BoxCollider2D boxCollider;

    [SerializeField]
    Vector2 rotationVector = Vector2.up;
    [SerializeField]
    float lastZrotation = float.NaN;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        InvalidateRotationVector();
    }

    void Start()
    {
        GameManager.instance.AddDoorToList(this);
    }

    void OnTriggerEnter2D(Collider2D collision) { CheckCollision(collision); }
    void OnTriggerStay2D(Collider2D collision) { CheckCollision(collision); }
    

    private void OnOpenDoorOut()
    {
            GameManager.instance.OnOpenDoor(this);
    }
      

    void Ready()
    {
        boxCollider.enabled = true;
    }

    void CheckCollision(Collider2D collision)
    {
        if (!boxCollider.enabled || GameManager.instance.isDlgShow) return;
        if (collision.gameObject.CompareTag("Player") ) {
            var playerMovement = collision.gameObject.GetComponent<Player>().inputVector;
            var dot = Vector2.Dot(playerMovement, rotationVector);
            if (dot < 0) {
                OnOpenDoorOut();
            }
        }
    }

    void InvalidateRotationVector()
    {
        if (Mathf.Approximately(transform.eulerAngles.z, lastZrotation)) return;
        lastZrotation = transform.eulerAngles.z;
        var defaultRotation = Vector2.up;
        var amount = lastZrotation * Mathf.Deg2Rad;
        var x = defaultRotation.x * Mathf.Cos(amount) - defaultRotation.y * Mathf.Sin(amount);
        if (Mathf.Abs(x) < 0.001f) x = 0;
        var y = defaultRotation.x * Mathf.Sin(amount) + defaultRotation.y * Mathf.Cos(amount);
        if (Mathf.Abs(y) < 0.001f) y = 0;
        rotationVector = new Vector2(x, y);
    }

    void OnDrawGizmos()
    {
        InvalidateRotationVector();
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position, 0.1f);
        var r = new Vector3(rotationVector.x, rotationVector.y, 0);
        Gizmos.DrawLine(transform.position, transform.position + (r * 0.4f));
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        InvalidateRotationVector();
        Gizmos.DrawSphere(transform.position, 0.1f);
        var r = new Vector3(rotationVector.x, rotationVector.y, 0);
        Gizmos.DrawLine(transform.position, transform.position + (r * 0.4f));
    }
}
