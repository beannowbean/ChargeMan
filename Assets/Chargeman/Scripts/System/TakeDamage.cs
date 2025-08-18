using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    //https://howudong.tistory.com/41
    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerTeleportMovement player = collision.GetComponent<PlayerTeleportMovement>();
        if (player is not null && player.CompareTag("Player"))
        {
            player.TakeDamage(damage); // 플레이어에게 1의 피해를 줌
        }
    }
}
