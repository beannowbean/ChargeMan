using UnityEngine;

public class Boss : MonoBehaviour
{
    public int hp = 10;
    public Sprite normalSprite;
    public Sprite BossDamaged;
    public float damagedTime = 0.2f;
    public Animator animator;

    private SpriteRenderer spriteRenderer;
    private float damagedTimer = 0f;
    private bool isDamaged = false;

    private void Awake()
    {
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 피격 상태일 때 시간 경과 체크
        if (isDamaged)
        {
            damagedTimer += Time.deltaTime; 

            if (damagedTimer >= damagedTime) 
            {
                spriteRenderer.sprite = normalSprite; 
                isDamaged = false; 
            }
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage; 
        Debug.Log("Boss HP: " + hp);

        animator.SetTrigger("isDamaged");

        if (hp <= 0)
        {
            Die(); 
        }
    }

    private void Die()
    {
        Debug.Log("Boss defeated!");
        Destroy(gameObject);
    }
}
