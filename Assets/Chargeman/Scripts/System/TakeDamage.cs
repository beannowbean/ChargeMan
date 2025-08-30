using UnityEngine;

public class TakeDamage : MonoBehaviour // 메서드랑 이름 헷갈릴 것 같은데 바꿔도 괜찮을지
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float destroyTime = 1.0f; //아마도 패턴에 따라 달라질 것

    [SerializeField] private float knockbackRate = 0;
    [SerializeField] Vector2 knockbackDir = Vector2.zero;
    [SerializeField] int effectId = 0;
    [SerializeField] float friction = 0;

    void Awake()
    {
        Destroy(this.gameObject, destroyTime);    
    }

    //https://howudong.tistory.com/41
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        Player player = collision.GetComponent<Player>();
        
        if (player is not null && collision.CompareTag("Player"))
        {
            player.OnHit(knockbackRate, knockbackDir, effectId, friction);
            player.TakeDamage(damage); // 플레이어에게 1의 피해를 줌
            Destroy(this.gameObject);
        }
    }



}
