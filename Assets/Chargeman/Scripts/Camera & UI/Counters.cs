using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Counters : MonoBehaviour
{
    public Player player;

    public TMP_Text moveCnt;
    public TMP_Text atkCnt;
    public TMP_Text chargeCnt;

    private void Awake()
    {
        moveCnt.SetText(player.MoveCnt.ToString());
        atkCnt.SetText(player.AtkCnt.ToString());
        chargeCnt.SetText(player.ChgCnt.ToString());
    }
    private void Update()
    {
        moveCnt.SetText(player.MoveCnt.ToString());
        atkCnt.SetText(player.AtkCnt.ToString());
        chargeCnt.SetText(player.ChgCnt.ToString());
    }
}
