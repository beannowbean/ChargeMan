using UnityEngine;

public class GrabAttack : EnemyAttack
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        damage = 0;
        knockbackDir = transform.position - collision.transform.position;
        knockbackRate = 10f;
        friction = 4f;
        base.OnTriggerEnter2D(collision);
    }
}
