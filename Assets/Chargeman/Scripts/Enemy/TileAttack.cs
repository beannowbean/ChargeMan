using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapExample : MonoBehaviour
{
    public Tilemap tilemap; // 타일맵 참조
    public Grid grid; // 그리드 참조
    public GameObject testObj;

    private float xOffSet = -8.5f; // 좌측 하단 모서리 타일을 (1,1)으로 취급하도록 함
    private float yOffset = -8.5f; // 나머지 타일은 1사분면으로 생각하면 됨

    void Start()
    {
        // 특정 셀 좌표 (예: (2, 3))의 월드 좌표 얻기
        Vector3Int cellPosition = new Vector3Int(2, 3, 0);
        Vector3 worldPosition = grid.CellToWorld(cellPosition);
        Debug.Log("Cell (" + cellPosition.x + ", " + cellPosition.y + ") World Position: " + worldPosition);


        // 월드 좌표 기준에 맞추기
        worldPosition.x += xOffSet;
        worldPosition.y += yOffset;

        Instantiate(testObj, worldPosition, Quaternion.identity);
    }
}