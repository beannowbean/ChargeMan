using UnityEngine;

public class SmallProjectileAttack : EnemyAttack
{
    public float speed = 10;

    public Vector2 dir = Vector2.left; // 기본방향 우측


    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = dir.normalized * speed;
        

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            player.OnHit(0,new Vector2(0,0),0,0); // 지금은 매개변수에 다 0넣었지만 나중에는 아예 매개변수 없는 함수도 만들거임!! (Test.cs 가면 대충 확인가능함)
            player.TakeDamage(damage); 
        }
        if(collision.tag != "Enemy") // 쌩 비교라 나중에 고치긴 해야할듯
            Destroy(this.gameObject);
    }

    public void SetTarget(Vector3 target)
    {
        if (target == null) return;
        dir = (target - transform.position).normalized;
    }

}
