using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour // 메서드랑 이름 헷갈릴 것 같은데 바꿔도 괜찮을지
{
    [SerializeField] public int damage = 1;
    //public int Damage { get { return damage; } set { damage = value; } }
    [SerializeField] public float destroyTime = 1.0f; //아마도 패턴에 따라 달라질 것

    [SerializeField] public float knockbackRate = 0;
    [SerializeField] public Vector2 knockbackDir = Vector2.zero;
    [SerializeField] public int effectId = 0;
    [SerializeField] public float friction = 0; // 

    void Awake()
    {
        if (destroyTime >= 0) // -1일때로 통일해서 무한지속 하나 만들죠?
            Destroy(this.gameObject, destroyTime);
    }

    //https://howudong.tistory.com/41
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        Player player = collision.GetComponent<Player>();
        
        if (player != null)
        {
            player.OnHit(knockbackRate, knockbackDir, effectId, friction); // 일단 임시로 Player.cs에 OnHit 메서드가 들어있음~~~~~
            player.TakeDamage(damage); // 플레이어에게 1의 피해를 줌
            Destroy(this.gameObject);
        }
    }



}
