using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float teleportDistance = 1f;
    SpriteRenderer spriteRenderer;
    Animator animator;
    [SerializeField] GameObject MovePoint;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        Instantiate(MovePoint, transform.position, Quaternion.identity);
    }

    private void Start()
    {
        
    }

    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    
    // 대시 관련
    public Vector3 moveDirection = Vector3.zero;
    bool canDash = true;
    float dashCooldown = 0.15f;

    private float curTime; // 공격 쿨타임 측정
    public float coolTime = 1f;
    public Transform pos;
    public Vector2 boxSize;

    public int chargeStack = 0;
    private int hp = 1;

    // 행동 횟수 제한 변수: 일단 10으로 통일함
    private int moveCount = 10;
    private int attackCount = 10;
    private int chargeCount = 10;

    // 테스트 여부 
    public bool isTesting  = true;

    private void Update()
    {

        TestMode();
        if (isTesting)
        {
            Charge();
            Move();
            if (chargeStack <= 0) // 스택이 0이면 대시/공격 불가
            {
                return;
            }
            Dash();
            Attack();
        }
        else // 기본 이동이 있는 거로 할지 -> 있어야됨
        {
            Move();
            if (moveCount <= 0 || !canDash) 
            {
                if (moveCount <= 0)
                    Debug.Log("이동 횟수 전부 소모");
                return;
            }
            
            else
            {
                Dash();
            }
            if (attackCount <= 0)
            {
                Debug.Log("공격 횟수 전부 소모");
                return;
            }
            else
            {
                Attack();
            }
            if (chargeCount <= 0)
            {
                Debug.Log("충전 횟수 전부 소모");
                return;
            }
            else
            {
                Charge();
            }
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"Player Hp : {hp}");

        if (hp <= 0) 
        {
            Debug.Log("Player Die");
            // isGameOver변수 
            // 테스트 모드에서는 안 끝나게
        }
    }

    private void Charge() // X키로 Charge
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            chargeStack++; // 스택 증가
            animator.SetTrigger("isCharge"); // Charge 시 스프라이트 변경
            Debug.Log("Charge Stack: " + chargeStack);
            if (!isTesting) chargeCount--;
        }
    }

    private void Move() // 기본 이동
    {
        if (!isTesting) moveSpeed = 1f;
        else moveSpeed = 5f;
        //Direction Sprite 플레이어 좌우 애니메이션
        //if (Input.GetButtonDown("Horizontal"))
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
    }

    private void Dash()
    {
        //Player character movement using teleport 캐릭터 방향키 순간이동
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        if (isTesting)
        {   // 기존형태
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
                animator.SetTrigger("isRolling");
                transform.position += moveDirection * teleportDistance;
                chargeStack--; // 이동 시 스택 차감
                Debug.Log("Charge Stack: " + chargeStack);
            }
        }
        else
        {
            moveDirection.x = Input.GetAxisRaw("Horizontal");
            moveDirection.y = Input.GetAxisRaw("Vertical");
            moveDirection = moveDirection.normalized;
            if (Input.GetKeyDown(KeyCode.Space))
                StartCoroutine(DashActivate());
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

                    if (isTesting)
                    {
                        if (boss != null)
                        {
                            boss.TakeDamage(1); // HP 1 깎기
                        }
                        chargeStack--; // 공격 시 스택 차감
                    }
                    else
                    {
                        if (boss != null)
                        {
                            boss.TakeDamage(1+chargeStack); 
                        }
                        chargeStack = 0;
                        attackCount--;
                        Debug.Log("Attack Stack: " + attackCount);
                    }
                }
                animator.SetTrigger("isAttack");
                curTime = coolTime;
                
                Debug.Log("Charge Stack: " + chargeStack);
            }

        }
        else
        {
            curTime -= Time.deltaTime;
        }
    }

    private void TestMode() // 키보드 1번 눌러서 이용 가능
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isTesting = !isTesting;
            Debug.Log("테스트 모드 변경. isTesting:" + isTesting);
        }
    }
    IEnumerator DelayTime(float time)
    {
        yield return new WaitForSeconds(time);
    }

    IEnumerator DashActivate()
    {
        canDash = false;
        transform.position += moveDirection * teleportDistance / 2 * (chargeStack + 1); // 충전 횟수에 비례하여 돌진 거리 증가
        chargeStack = 0;
        Debug.Log("Move Stack: " + moveCount);
        moveCount--;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
