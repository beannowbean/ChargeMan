using UnityEngine;

public class DefaultAttack : TakeDamage // 아 그래 이맛이야
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
