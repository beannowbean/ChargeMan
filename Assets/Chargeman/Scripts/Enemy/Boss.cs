using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Shapes2D;
using System;

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
            xSize = ySize = 6;
            //StartCoroutine(CircleWarn(xSize, ySize,1));
            StartCoroutine(ScaleUsingPattern(0, new Vector3(0f, 0f, 1f), new Vector3(6f, 6f, 1f), 2));
        }

        if (Input.GetKeyDown(KeyCode.O) && isDoing == false) // 직선
        {
            isDoing = true;
            patternNum = 1;
            xSize = 1;
            ySize = 1;
            //StartCoroutine(LineWarn(xSize, ySize, 1));
            StartCoroutine(ScaleUsingPattern(1, new Vector3(0f, 1f, 1f), new Vector3(1f, 1f, 1f), 1));
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

        if (Input.GetKeyDown(KeyCode.U) && isDoing == false)
        {
            isDoing = true;
            patternNum = 3;
            xSize = 10;
            ySize = 10;
            StartCoroutine(Laser(new Vector3(0f, 0f, -50f), new Vector3(0f, 0f, 50f), 0.9f, 0.5f));
            StartCoroutine(ArcWarn(new Vector3(xSize, ySize, 1f), 40, 40, 41, 140, 0.6f));
        }


        //if (isDoing) StartCoroutine(CircleWarning());
        if (isDone)
        {
            //StopAllCoroutines();

            if (patternNum == 2)
            {
                GameObject Pattern = Instantiate(pattern[patternNum], new Vector3(xPos + xOffSet, yPos + yOffset, 0f), Quaternion.identity);
                Pattern.transform.localScale = new Vector3(xSize, ySize, 0);
                isDoing = false;
                isDone = false;
            }
            else if (patternNum == 3)
            {
               // StartCoroutine(Laser());
                isDoing = false;
                isDone = false;
            }
            else
            {
                GameObject Pattern = Instantiate(pattern[patternNum], this.transform);
                Pattern.transform.localScale = new Vector3(xSize, ySize, 0);
                isDoing = false;
                isDone = false;
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

    private IEnumerator ScaleUsingPattern(int patternType, Vector3 startScale, Vector3 endScale, float delay)
    {
        float currentTime = 0;

        Transform size = warning[patternType].GetComponent<Transform>();
        while (currentTime < delay)
        {
            currentTime += Time.deltaTime;
            size.localScale = Vector3.Lerp(startScale, endScale, currentTime / delay);
            yield return null;
        }
        isDone = true;
        size.localScale = new Vector3(0f, 0f, 1f);
        yield break;
    }

    private IEnumerator PointWarn(float x, float y, float patternDelay)
    {

        Transform size = warning[2].GetComponent<Transform>();
        size.position = new Vector3(x + xOffSet, y + yOffset, 0f);
        Debug.Log(x + "," + y);
        while (size.localScale.x < 1)
        {
            size.localScale += new Vector3(patternDelay * 2f * Time.deltaTime, patternDelay * 2f * Time.deltaTime, 0f);
            yield return null;
        }
        isDone = true;
        size.localScale = new Vector3(0f, 0f, 1f);
        yield break;
    }

    private IEnumerator ArcWarn(Vector3 size, float startAngleL, float endAngleL, float startAngleR, float endAngleR, float delay)
    {
        float currentTime = 0;
        warning[3].GetComponent<Transform>().localScale = size;
        Shape.UserProps shape = warning[3].GetComponent<Shape>().settings;

        yield return new WaitForSeconds(0.3f);
        warning[3].SetActive(true);
        while (currentTime < delay)
        {
            currentTime += Time.deltaTime;
            shape.startAngle = Mathf.Lerp(startAngleL, endAngleL, currentTime / delay);
            shape.endAngle = Mathf.Lerp(startAngleR, endAngleR, currentTime / delay);
            yield return null;
        }
        isDone = true;
        shape.startAngle = 40;
        shape.endAngle = 41;
        warning[3].SetActive(false);
        yield break;
    }

    private IEnumerator Laser(Vector3 startRotation, Vector3 endRotation, float turnOnDelay, float turnOffDelay)
    {
        float currentTime = 0;
        Transform tr = pattern[3].GetComponent<Transform>();
        SpriteRenderer sr = pattern[3].GetComponent<SpriteRenderer>();
        Animator ani = pattern[3].GetComponent<Animator>();
        tr.localRotation = Quaternion.Euler(startRotation);

        pattern[3].SetActive(true);
        ani.SetBool("isActive", true);

        yield return new WaitForSeconds(0.8f);

        while (currentTime < turnOffDelay)
        {
            currentTime += Time.deltaTime;
            tr.localRotation = Quaternion.Slerp(Quaternion.Euler(startRotation), Quaternion.Euler(endRotation), currentTime / turnOffDelay);
            yield return null;
        }

        ani.SetBool("isActive", false);

        pattern[3].SetActive(false);

        yield break;
    }
}
