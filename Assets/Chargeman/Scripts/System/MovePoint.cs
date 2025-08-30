using UnityEngine;

public class MovePoint : MonoBehaviour
{
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 입력값에 따라 대시할 위치로 마크 이동
    }

    void DashInfo()
    {
        Player charcter = player.GetComponent<Player>();
        // charcter.
    }
}
