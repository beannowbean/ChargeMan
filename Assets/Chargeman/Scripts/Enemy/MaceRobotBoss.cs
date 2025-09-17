using UnityEngine;
using Shapes2D;
using System.Collections;

public class MaceRobotBoss : MonoBehaviour, IBoss
{
    private int _hp = 10;
    public int Hp { get { return _hp; } set { _hp = value; } }

    private int _maxHp = 10;
    public int MaxHp { get { return _maxHp; } }

    private int _patternNum = -1;
    private float _moveSpeed = 0;
    private float _damagedTimer = 0f;
    private float _xOffSet = -8.5f; // 좌측 하단 모서리 타일을 (1,1)으로 취급하도록 함
    private float _yOffset = -8.5f; // 나머지 타일은 1사분면으로 생각하면 됨

    private bool _isDamaged = false;
    private bool _isDone = false;
    private bool _isDoing = false;

    [SerializeField] private float _damagedTime = 0.2f;
    [SerializeField] private Sprite _normalSprite;
    [SerializeField] private Sprite _damagedSprite;
    [SerializeField] private GameObject[] warning;
    [SerializeField] private GameObject[] pattern;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private bool istesting = true;
    public bool isTesting { get { return istesting; } }

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDamaged)
        {
            _damagedTimer += Time.deltaTime;

            if (_damagedTimer >= _damagedTime)
            {
                _spriteRenderer.sprite = _normalSprite;
                _isDamaged = false;
            }
        }

        //패턴 테스트
        if (Input.GetKeyDown(KeyCode.P) && _isDoing == false) // 원형
        {
            _isDoing = true;
            _patternNum = 0;
            StartCoroutine(ScaleUsingPattern(0, new Vector3(0f, 0f, 1f), new Vector3(6f, 6f, 1f), 2));
        }

        if (Input.GetKeyDown(KeyCode.O) && _isDoing == false) // 직선
        {
            _isDoing = true;
            _patternNum = 1;
            StartCoroutine(ScaleUsingPattern(1, new Vector3(0f, 1f, 1f), new Vector3(1f, 1f, 1f), 1));
        }

        if (Input.GetKeyDown(KeyCode.I) && _isDoing == false) // 좌표
        {
            _isDoing = true;
            _patternNum = 2;
            StartCoroutine(PointWarn(8, 5, new Vector3(0f, 0f, 1f), new Vector3(1f, 1f, 1f), 0.5f));
        }

        if (Input.GetKeyDown(KeyCode.U) && _isDoing == false)
        {
            _isDoing = true;
            _patternNum = 3;
            StartCoroutine(Laser(new Vector3(0f, 0f, -50f), new Vector3(0f, 0f, 50f), 0.9f, 0.5f));
            //StartCoroutine(ArcWarn(new Vector3(xSize, ySize, 1f), 40, 40, 41, 140, 0.6f));
        }

        _animator.SetInteger("patternNum", _patternNum);
    }

    public void TakeDamage(int damage)
    {
        _hp -= damage;
        Debug.Log("Boss HP: " + _hp);
        istesting = false;

        _animator.SetTrigger("isDamaged");

        if (_hp <= 0)
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
        Debug.Log(_patternNum);
        float currentTime = 0;

        Transform size = warning[patternType].GetComponent<Transform>();
        while (currentTime < delay)
        {
            currentTime += Time.deltaTime;
            size.localScale = Vector3.Lerp(startScale, endScale, currentTime / delay);
            yield return null;
        }
        size.localScale = new Vector3(0f, 0f, 1f);

        GameObject attack = Instantiate(pattern[patternType], this.transform);
        attack.transform.localScale = endScale;

        _isDoing = false;
        _patternNum = -1;
        Debug.Log(_patternNum);
        yield break;
    }

    private IEnumerator PointWarn(float x, float y, Vector3 startScale, Vector3 endScale, float patternDelay)
    {
        float currentTime = 0;

        Transform size = warning[2].GetComponent<Transform>();
        size.position = new Vector3(x + _xOffSet, y + _yOffset, 0f);
        Debug.Log(x + "," + y);
        while (size.localScale.x < 1)
        {
            currentTime += Time.deltaTime;
            size.localScale = Vector3.Lerp(startScale, endScale, currentTime / patternDelay);
            yield return null;
        }
        size.localScale = new Vector3(0f, 0f, 1f);

        GameObject attack = Instantiate(pattern[2], new Vector3(x + _xOffSet, y + _yOffset, 0f), Quaternion.identity);
        attack.transform.localScale = endScale;

        _isDoing = false;
        yield break;
    }

    private IEnumerator ArcWarn(Vector3 size, float startAngleLeft, float endAngleLeft, float startAngleRight, float endAngleRight, float delay)
    {
        float currentTime = 0;
        warning[3].GetComponent<Transform>().localScale = size;
        Shape.UserProps shape = warning[3].GetComponent<Shape>().settings;

        yield return new WaitForSeconds(0.3f);
        warning[3].SetActive(true);
        while (currentTime < delay)
        {
            currentTime += Time.deltaTime;
            shape.startAngle = Mathf.Lerp(startAngleLeft, endAngleLeft, currentTime / delay);
            shape.endAngle = Mathf.Lerp(startAngleRight, endAngleRight, currentTime / delay);
            yield return null;
        }

        shape.startAngle = 40;
        shape.endAngle = 41;
        warning[3].SetActive(false);

        _isDoing = false;
        yield break;
    }

    private IEnumerator Laser(Vector3 startRotation, Vector3 endRotation, float turnOnDelay, float turnOffDelay)
    {
        float currentTime = 0;
        Transform tr = pattern[3].GetComponent<Transform>();
        Animator ani = pattern[3].GetComponent<Animator>();
        tr.localRotation = Quaternion.Euler(startRotation);

        pattern[3].SetActive(true);
        ani.SetBool("isActive", true);

        yield return new WaitForSeconds(turnOnDelay);

        while (currentTime < turnOffDelay)
        {
            currentTime += Time.deltaTime;
            tr.localRotation = Quaternion.Slerp(Quaternion.Euler(startRotation), Quaternion.Euler(endRotation), currentTime / turnOffDelay);
            yield return null;
        }

        ani.SetBool("isActive", false);

        pattern[3].SetActive(false);

        StartCoroutine(ArcWarn(new Vector3(10, 10, 1f), 40, 40, 41, 140, 0.6f));

        yield break;
    }

    private IEnumerator MaceCrash()
    {
        yield break;
    }
}
