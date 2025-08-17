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
    //private bool isTesting = false; //패턴 테스트를 위한 변수

    [SerializeField] private GameObject[] warning;
    [SerializeField] private GameObject[] pattern;

    private void Awake()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        circleWarnAnimator = warning[0].GetComponent<Animator>();
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
        /*
        if (Input.GetKeyDown(KeyCode.P) && isDoing == false)
        {
            Transform tr = warning[0].GetComponent<Transform>();
            tr.localScale = new Vector3(0f, 0f, 1f);
            isDoing = true;
            StartCoroutine(CircleWarning());
        }

        //if (isDoing) StartCoroutine(CircleWarning());
        if (isDone)
        {
            isDone = false;
            isDoing = false;
            StopCoroutine(CircleWarning());
        }*/
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

    private IEnumerator CircleWarn()
    {
        
    }
}
