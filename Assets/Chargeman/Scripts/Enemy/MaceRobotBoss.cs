using UnityEngine;
using Shapes2D;
using System.Collections;
using System.Collections.Generic;

public class MaceRobotBoss : MonoBehaviour, IBoss
{
    [SerializeField] private GameObject _player;
    private Transform _playerTransform;

    private int _hp = 10;
    public int Hp { get { return _hp; } set { _hp = value; } }

    private int _maxHp = 10;
    public int MaxHp { get { return _maxHp; } }

    private float _cooldown;
    [SerializeField] private float _cooldownMax = 5f;

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
    
    private List<IEnumerator> _patternList = new List<IEnumerator>();

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private bool istesting = true;
    public bool isTesting { get { return istesting; } }

    void Awake()
    {
        _cooldown = _cooldownMax;

        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerTransform = _player.GetComponent<Transform>();

        _patternList.Add(MaceCrash(new Vector3(0f, 0f, 1f), new Vector3(4f, 3f, 1f), 2f));
        _patternList.Add(Shoot(_playerTransform));
        _patternList.Add(Laser(new Vector3(0f, 0f, -50f), new Vector3(0f, 0f, 50f), 0.9f, 0.5f));
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

        _animator.SetInteger("patternNum", _patternNum);

        _cooldown -= Time.deltaTime; //패턴 쿨타임 감소
        //Debug.Log(_cooldown);

        //패턴 시전
        //float distance = Vector3.Distance(_player.transform.localPosition, this.transform.localPosition);
        if (_cooldown <= 0 && !_isDoing)
        {
            int type = Random.Range(0, 3);
            //StartCoroutine(_patternList[type]);
            switch (type)
            {
                case 0:
                    StartCoroutine(MaceCrash(new Vector3(0f, 0f, 1f), new Vector3(4f, 3f, 1f), 2f));
                    break;
                case 1:
                    StartCoroutine(Shoot(_playerTransform));
                    break;
                case 2:
                    StartCoroutine(Laser(new Vector3(0f, 0f, -50f), new Vector3(0f, 0f, 50f), 0.9f, 0.5f));
                    break;
            }

            _isDoing = true;
        }
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
        float currentTime = 0;

        GameObject warningRange = Instantiate(warning[patternType], this.transform.localPosition, Quaternion.identity);

        Transform size = warningRange.GetComponent<Transform>();
        while (currentTime < delay)
        {
            currentTime += Time.deltaTime;
            size.localScale = Vector3.Lerp(startScale, endScale, currentTime / delay);
            yield return null;
        }

        Destroy(warningRange);
        /*
        GameObject attack = Instantiate(pattern[patternType], this.transform.localPosition, Quaternion.identity);
        attack.transform.localScale = endScale;*/
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
        yield break;
    }

    private IEnumerator ArcWarn(Vector3 size, float startAngleLeft, float endAngleLeft, float startAngleRight, float endAngleRight, float delay)
    {
        Debug.Log("Laser");
        float currentTime = 0;
        GameObject warningRange = Instantiate(warning[3], this.transform.localPosition, Quaternion.identity);
        warningRange.GetComponent<Transform>().localScale = size;
        Shape.UserProps shape = warningRange.GetComponent<Shape>().settings;

        yield return new WaitForSeconds(0.3f);
        //warning[3].SetActive(true);
        while (currentTime < delay)
        {
            currentTime += Time.deltaTime;
            shape.startAngle = Mathf.Lerp(startAngleLeft, endAngleLeft, currentTime / delay);
            shape.endAngle = Mathf.Lerp(startAngleRight, endAngleRight, currentTime / delay);
            yield return null;
        }

        shape.startAngle = 40;
        shape.endAngle = 41;
        //warning[3].SetActive(false);

        Destroy(warningRange);
        _isDoing = false;
        yield break;
    }

    private IEnumerator Laser(Vector3 startRotation, Vector3 endRotation, float turnOnDelay, float turnOffDelay)
    {
        float currentTime = 0;
        GameObject laser = Instantiate(pattern[3], this.transform.localPosition, Quaternion.identity);
        Transform tr = laser.GetComponent<Transform>();
        Animator ani = laser.GetComponent<Animator>();
        tr.localRotation = Quaternion.Euler(startRotation);

        //pattern[3].SetActive(true);
        ani.SetBool("isActive", true);

        StartCoroutine(ArcWarn(new Vector3(10, 10, 1f), 40, 40, 41, 140, 0.6f));

        yield return new WaitForSeconds(turnOnDelay);

        yield return new WaitForSeconds(0.6f);

        while (currentTime < turnOffDelay)
        {
            currentTime += Time.deltaTime;
            tr.localRotation = Quaternion.Slerp(Quaternion.Euler(startRotation), Quaternion.Euler(endRotation), currentTime / turnOffDelay);
            yield return null;
        }

        ani.SetBool("isActive", false);

        //pattern[3].SetActive(false);
        Destroy(laser);
        _cooldown = _cooldownMax;

        _isDoing = false;

        yield break;
    }

    private IEnumerator MaceCrash(Vector3 startScale, Vector3 endScale, float delay)
    {
        Debug.Log("Crash");
        IEnumerator ie = ScaleUsingPattern(0, startScale, endScale, delay);
        StartCoroutine(ie);

        yield return new WaitForSeconds(delay);

        GameObject attack = Instantiate(pattern[0], this.transform.localPosition, Quaternion.identity);
        attack.transform.localScale = endScale;

        _isDoing = false;

        _cooldown = _cooldownMax;

        yield break;
    }

    private IEnumerator Shoot(Transform Target)
    {
        Debug.Log("Shoot");
        GameObject Projectile = Instantiate(pattern[4], this.transform.localPosition, Quaternion.identity);
        Projectile.GetComponent<SmallProjectileAttack>().SetTarget(Target);

        _isDoing = false;

        _cooldown = _cooldownMax;
        yield break;
    }
}
