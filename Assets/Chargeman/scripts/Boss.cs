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
            isDoing = true;
            StartCoroutine(CircleWarn());
        }

        //if (isDoing) StartCoroutine(CircleWarning());
        if (isDone)
        {
            isDone = false;
            isDoing = false;
            StopCoroutine(CircleWarn());

            Transform tr = warning[0].GetComponent<Transform>();
            tr.localScale = new Vector3(0f, 0f, 1f);
            Instantiate(pattern[0], this.transform);
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

    private IEnumerator CircleWarn()
    {
        Transform size = warning[0].GetComponent<Transform>();
        while (size.localScale.x < 10.0f)
        {
            size.localScale += new Vector3(2f * Time.deltaTime, 2f * Time.deltaTime, 0f);
            yield return null;
            //yield return new WaitForSeconds(0.1f);
        }
        isDone = true;
        yield return null;
    }
}
