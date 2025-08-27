using System.Collections;
using System.Collections.Generic;
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

    private float xOffSet = -8.5f; // 좌측 하단 모서리 타일을 (1,1)으로 취급하도록 함
    private float yOffset = -8.5f; // 나머지 타일은 1사분면으로 생각하면 됨

    private float xPos;
    private float yPos;

    [SerializeField] private GameObject[] warning;
    [SerializeField] private GameObject[] pattern;

    Vector2[] testList = new Vector2[5] {new Vector2(8, 5),
                                         new Vector2(8, 6),
                                         new Vector2(7, 5),
                                         new Vector2(8, 7),
                                         new Vector2(9, 5)};

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

        if (Input.GetKeyDown(KeyCode.P) && isDoing == false) // 원형
        {
            Debug.Log("버튼 눌림");
            isDoing = true;
            patternNum = 0; // 코루틴 안에서 patternNum 바꾸는 건 어떰
            xSize = ySize = 4;
            StartCoroutine(CircleWarn(xSize, ySize, 1));
        }

        if (Input.GetKeyDown(KeyCode.O) && isDoing == false) // 직선
        {
            isDoing = true;
            patternNum = 1;
            xSize = 2;
            ySize = 1;
            StartCoroutine(LineWarn(xSize, ySize, 1));
        }

        if (Input.GetKeyDown(KeyCode.I) && isDoing == false) // 좌표
        {
            isDoing = true;
            patternNum = 2;
            xSize = 1;
            ySize = 1;
            xPos = 8;
            yPos = 5;
            StartCoroutine(PointWarn(xPos, yPos, 0.5f));
        }

        if (Input.GetKeyDown(KeyCode.U) && isDoing == false) // 좌표 연속
        {
            isDoing = true;
            patternNum = 3;
            xSize = 1;
            ySize = 1;
            xPos = 8;
            yPos = 5;
            StartCoroutine(TestPattern(5, testList, 0.5f, 0.5f));
        }


        //if (isDoing) StartCoroutine(CircleWarning());
        if (isDone)
        {
            isDone = false;
            isDoing = false;
            //StopAllCoroutines();

            if (patternNum == 2)
            {
                GameObject Pattern = Instantiate(pattern[patternNum], new Vector3(xPos + xOffSet, yPos + yOffset, 0f), Quaternion.identity);
                Pattern.transform.localScale = new Vector3(xSize, ySize, 0);
            }
            else
            {
                GameObject Pattern = Instantiate(pattern[patternNum], this.transform);
                Pattern.transform.localScale = new Vector3(xSize, ySize, 0);
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

    //나중에 코루틴 통합할 수 있으면 할 예정
    private IEnumerator CircleWarn(float x, float y, float patternDelay)
    {
        Transform size = warning[0].GetComponent<Transform>();
        //size.localScale = new Vector3(0f, 0f, 1f);
        while (size.localScale.x < x)
        {
            size.localScale += new Vector3(patternDelay * 2f * Time.deltaTime, patternDelay * 2f * Time.deltaTime, 0f);
            yield return null;
        }
        isDone = true;
        size.localScale = new Vector3(0f, 0f, 1f);
        yield break;
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
        yield break;
    }

    private IEnumerator PointWarn(float x, float y, float patternDelay)
    {
        GameObject warnObject = Instantiate(warning[2]);
        Transform size = warnObject.GetComponent<Transform>();
        size.position = new Vector3(x + xOffSet, y + yOffset, 0f);
        Debug.Log(x + "," + y);
        while (size.localScale.x < 1)
        {
            size.localScale += new Vector3(patternDelay * 2f * Time.deltaTime, patternDelay * 2f * Time.deltaTime, 0f);
            yield return null;
        }
        //isDone = true;
        size.localScale = new Vector3(0f, 0f, 1f);
        Destroy(warnObject);

        GameObject Pattern = Instantiate(pattern[2], new Vector3(xPos + xOffSet, yPos + yOffset, 0f), Quaternion.identity);
        Pattern.transform.localScale = new Vector3(xSize, ySize, 0);
        yield break;
    }

    private IEnumerator TestPattern(int count, Vector2[] positions, float warnDelay, float patternTerm)
    {
        List<IEnumerator> enumerators = new List<IEnumerator>();

        for (int i = 0; i < count; i++)
        {
            enumerators.Add(PointWarn(positions[i].x, positions[i].y, warnDelay));
        }

        for (int i = 0; i < count; i++)
        {
            StartCoroutine(enumerators[i]);
            yield return new WaitForSeconds(patternTerm);
        }

        isDoing = false;
        yield break;
    }
}
