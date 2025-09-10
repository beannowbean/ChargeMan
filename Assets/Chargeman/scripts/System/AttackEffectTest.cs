using UnityEngine;

public class AttackEffectTest : MonoBehaviour
{
    [SerializeField] private Transform _effectSpriteTr;
    private Transform _tr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _tr.localScale -= new Vector3(5f * Time.deltaTime, 5f * Time.deltaTime, 0f);
            if (_tr.localScale.x < 5f)
            {
                _tr.localScale = new Vector3(5f, 5f, 1f);
            } 
        }
        else
        {
            _tr.localScale += new Vector3(5f * Time.deltaTime, 5f * Time.deltaTime, 0f);
            if (_tr.localScale.x > 21f)
            {
                _tr.localScale = new Vector3(21f, 21f, 1f);
            }
        }
    }
}
