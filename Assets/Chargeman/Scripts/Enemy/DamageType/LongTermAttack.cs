using UnityEngine;

public class LongTermAttack : EnemyAttack
{
    private float cycle = 1f;
    private float time = 0f;
    // 더미로 스택 하나 만들어봄
    [SerializeField] private int stack = 0;
    public override void OnTriggerEnter2D(Collider2D collision){ time = 0; }
    public void OnTriggerStay2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        time += Time.deltaTime;
        if (time >= cycle)
        {
            time = 0f;
            player.OnHit(knockbackRate, knockbackDir, 1, friction);
            stack++;
            if (stack == 3) player.TakeDamage(1);
        }
    }
    
}
