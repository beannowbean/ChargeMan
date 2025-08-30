using UnityEngine;

public class ChargeGauge : MonoBehaviour // 충전 게이지
{ 

    public GameObject p;
    Player player;
    private RectTransform rectTransform;
    private int Maximum;
    int unit;

    private void Awake()
    {
        if (!rectTransform) rectTransform = GetComponent<RectTransform>();
        if (!player)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go) player = go.GetComponent<Player>();
        }
    }

    private void Start()
    {

        Maximum = player.GetMaxCharge();
        unit = 500 / Maximum;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 0);
        Debug.Log($"fill={rectTransform}, hasCanvas={GetComponentInParent<Canvas>() != null}", this);
    }
    private void Update()
    {
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, unit*player.chargeStack);
        Debug.Log(player.chargeStack);
    }
}
