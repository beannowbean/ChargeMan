using UnityEngine;

public class MaceRobotBoss : MonoBehaviour, IBoss
{
    private int hp = 10;
    public int Hp { get { return hp; } set { hp = value; } }

    private float moveSpeed = 0;
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
    }
}
