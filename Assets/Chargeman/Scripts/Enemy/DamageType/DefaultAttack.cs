using Unity.VisualScripting;
using UnityEngine;

public class DefaultAttack : EnemyAttack // �� �׷� �̸��̾�
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
