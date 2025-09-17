using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject bar;
    [SerializeField] private Vector3 margin = new Vector3(0, 1, 0);

    private Transform tr;
    private IBoss bossScript;
    private Image barImage;

    private float time = 0;

    void Awake()
    {
        barImage = bar.GetComponent<Image>();
        tr = GetComponent<Transform>();
        bossScript = boss.GetComponent<IBoss>();
    }

    // Update is called once per frame
    void Update()
    {
        tr.position = Camera.main.WorldToScreenPoint(boss.transform.position + margin);
        barImage.fillAmount = (float)bossScript.Hp / bossScript.MaxHp;
    }
}
