using UnityEngine;

public class Player : MonoBehaviour
{
    public float teleportDistance = 2f;
    SpriteRenderer spriteRenderer;
    Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }


    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;


    private float curTime;
    public float coolTime = 1f;
    public Transform pos;
    public Vector2 boxSize;

    private int chargeStack = 0;
    private int hp = 0;

    private void Update()
    {
       
        Charge();
        Move();
        if (chargeStack <= 0) // 스택이 0이면 이동/공격 불가
        {
            return;
        }
        Dash();
        Attack();
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"Player Hp : {hp}");

        if (hp <= 0) Debug.Log("Player Die");
    }

    private void Charge() // X키로 Charge
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            chargeStack++; // 스택 증가
            animator.SetTrigger("isCharge"); // Charge 시 스프라이트 변경
            Debug.Log("Charge Stack: " + chargeStack);
        }
    }

    private void Move() // 기본 이동
    {
        //Direction Sprite 플레이어 좌우 애니메이션
        if (Input.GetButtonDown("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
    }

    private void Dash()
    {
        //Player character movement using teleport 캐릭터 방향키 순간이동
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKeyDown(KeyCode.LeftShift))
            moveDirection = Vector3.up;
        else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.LeftShift))
            moveDirection = Vector3.down;
        else if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKeyDown(KeyCode.LeftShift))
            moveDirection = Vector3.left;
        else if (Input.GetKey(KeyCode.RightArrow) && Input.GetKeyDown(KeyCode.LeftShift))
            moveDirection = Vector3.right;

        if (moveDirection != Vector3.zero)
        {
            transform.position += moveDirection * teleportDistance;
            chargeStack--; // 이동 시 스택 차감
            Debug.Log("Charge Stack: " + chargeStack);
        }
    }

    private void Attack() // Player Attack 플레이어 공격
    {
        if (curTime <= 0)
        {
            //공격
            if (Input.GetKeyDown(KeyCode.Z))
            {

                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);

                foreach (Collider2D collider in collider2Ds)
                {
                    Boss boss = collider.GetComponent<Boss>(); // BOSS 스크립트 찾기

                    if (boss != null)
                    {
                        boss.TakeDamage(1); // HP 1 깎기
                    }
                }


                animator.SetTrigger("isAttack");
                curTime = coolTime;
                chargeStack--; // 공격 시 스택 차감
                Debug.Log("Charge Stack: " + chargeStack);
            }

        }
        else
        {
            curTime -= Time.deltaTime;
        }
    }

}
