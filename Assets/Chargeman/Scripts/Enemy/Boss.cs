using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Boss : MonoBehaviour
{
    public int hp = 10;
    public Sprite normalSprite;
    public Sprite BossDamaged;
    public float damagedTime = 0.2f;
    public Animator animator;
    private Animator circleWarnAnimator;

    private SpriteRenderer spriteRenderer;
    private float damagedTimer = 0f;
    private bool isDamaged = false;
    private bool isDone = false;
    private bool isDoing = false;
    private int patternNum = 0;
    //private bool isTesting = false; //패턴 테스트를 위한 변수

    // 패턴 시전시 크기 조절용 변수
    private float xSize = 0;
    private float ySize = 0;

    [SerializeField] private GameObject[] warning;
    [SerializeField] private GameObject[] pattern;

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

        if (Input.GetKeyDown(KeyCode.P) && isDoing == false)
        {
            Debug.Log("버튼 눌림");
            isDoing = true;
            patternNum = 0; // 코루틴 안에서 patternNum 바꾸는 건 어떰
            xSize = ySize = 10;
            StartCoroutine(CircleWarn(xSize, ySize,1));
        }

        if (Input.GetKeyDown(KeyCode.O) && isDoing == false)
        {
            isDoing = true;
            patternNum = 1;
            xSize = 2;
            ySize = 1;
            StartCoroutine(LineWarn(xSize, ySize, 1));
        }

        //if (isDoing) StartCoroutine(CircleWarning());
        if (isDone)
        {
            isDone = false;
            isDoing = false;
            StopAllCoroutines();
            GameObject Pattern = Instantiate(pattern[patternNum], this.transform);
            Pattern.transform.localScale = new Vector3(xSize, ySize, 0);
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

    //나중에 코루틴 통합할 수 있으면 할 예정
    private IEnumerator CircleWarn(float x, float y, float patternDelay)
    {
        Transform size = warning[0].GetComponent<Transform>();
        //size.localScale = new Vector3(0f, 0f, 1f);
        while (size.localScale.x < x)
        {
            size.localScale += new Vector3(patternDelay * 2f * Time.deltaTime, patternDelay * 2f * Time.deltaTime, 0f);
            yield return null;
            //yield return new WaitForSeconds(0.1f);
        }
        isDone = true;
        size.localScale = new Vector3(0f, 0f, 1f);
        yield return null;
    }

    private IEnumerator LineWarn(float x, float y, float patternDelay)
    {
        Transform size = warning[1].GetComponent<Transform>();
        //size.localScale = new Vector3(0f, 1f, 1f);
        while (size.localScale.x < x)
        {
            size.localScale += new Vector3(patternDelay * 0.2f * Time.deltaTime, 0f, 0f);
            yield return null;
        }
        isDone = true;
        size.localScale = new Vector3(0f, 1f, 1f);
        yield return null;
    }
}
