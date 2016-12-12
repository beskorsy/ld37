using UnityEngine;
using System.Collections;

public class Slash : MonoBehaviour
{

    static GameObject NearTarget(Vector3 position, Collider2D[] array)
    {
        Collider2D current = null;
        float dist = Mathf.Infinity;

        foreach (Collider2D coll in array)
        {
            float curDist = Vector3.Distance(position, coll.transform.position);

            if (curDist < dist)
            {
                current = coll;
                dist = curDist;
            }
        }

        return current.gameObject;
    }

    public static void ActionPlayer(Vector2 point, float radius, int damage, bool allTargets)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(point, radius);

        if (!allTargets)
        {
            GameObject obj = NearTarget(point, colliders);
            if (obj.GetComponent<Enemy>())
            {
                obj.GetComponent<Enemy>().Damage(damage);
            }
            return;
        }

        foreach (Collider2D hit in colliders)
        {
            if (hit.GetComponent<Enemy>())
            {
                hit.GetComponent<Enemy>().Damage(damage);
            }
        }
    }

    public static bool TryActionEnemy(Vector2 point, float radius, int damage)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(point, radius);

        foreach (Collider2D hit in colliders)
        {
            Player player = hit.GetComponent<Player>();
            if (player != null && player.hp > 0)
            {
                player.Damage(damage);
                return true;
            }
        }

        return false;
    }
}
