using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float destroyTime = 1.0f; //아마도 패턴에 따라 달라질 것


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
            player.TakeDamage(damage); // 플레이어에게 1의 피해를 줌
            Destroy(this.gameObject);
        }
    }
}
