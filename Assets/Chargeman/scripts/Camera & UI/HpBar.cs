using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject bar;

    private Transform tr;
    private IBoss bossScript;
    private Image barImage;

    void Awake()
    {
        barImage = bar.GetComponent<Image>();
        tr = GetComponent<Transform>();
        bossScript = boss.GetComponent<IBoss>();
    }

    // Update is called once per frame
    void Update()
    {
        tr.position = Camera.main.WorldToScreenPoint(boss.transform.position + Vector3.up);
        barImage.fillAmount = (float)bossScript.Hp / bossScript.MaxHp;
    }
}
