using UnityEngine;

public class MovePoint : MonoBehaviour
{
    [SerializeField] private Player player;         // Player 컴포넌트 참조
    [SerializeField] private float previewScale = 0.5f;

    private Vector2 lastDir = Vector2.right;        // 시작 기본 방향

    void Awake()
    {
        if (player == null) player = FindFirstObjectByType<Player>();
    }

    void LateUpdate()
    {
        // 1) Player가 이미 moveDirection을 계산해 둔 경우를 우선 사용
        Vector2 dir = player != null ? (Vector2)player.moveDirection : Vector2.zero;

        // 2) (선택) 만약 Player에서 방향을 안 주면 키 상태로 계산
        if (dir == Vector2.zero)
        {
            int x = (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) - (Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);
            int y = (Input.GetKey(KeyCode.UpArrow) ? 1 : 0) - (Input.GetKey(KeyCode.DownArrow) ? 1 : 0);
            dir = new Vector2(x, y);
        }

        // 입력이 있을 때만 마지막 방향 갱신 (대각선도 그대로 normalizing)
        if (dir.sqrMagnitude > 0f)
            lastDir = dir.normalized;

        float dist = player.teleportDistance * previewScale * (player.chargeStack + 1);

        // 플레이어 앞 dist만큼 위치 + 회전(Atan2, 대각 정확)
        Vector3 targetPos = player.transform.position + (Vector3)(lastDir * dist);
        transform.position = targetPos;

        float angle = Mathf.Atan2(lastDir.y, lastDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
