using UnityEngine;

namespace Platformer
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target; // 

        public Vector3 offset = new Vector3(0, 0, -10);

        public float cameraSpeed = 5f;

        // Update is called once per frame
        void FixedUpdate() // 물리 연산과 싱크 맞추기 위해 Update 대신
        {
            Vector3 targetPos = target.position + offset; // 위치 + 보정치
            Vector3 resultPos = Vector3.Lerp(transform.position, targetPos, cameraSpeed * Time.fixedDeltaTime);
            transform.position = resultPos;
        }
    }

}

