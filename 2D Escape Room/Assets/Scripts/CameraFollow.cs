using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // 플레이어의 Transform을 드래그해서 연결하거나 코드에서 지정

    public Vector3 offset;  // 카메라와 플레이어 사이의 고정된 거리

    public float smoothSpeed = 0.125f;
    void Update()
    {
        if (player != null)
        {
            // 플레이어 위치에 오프셋을 더해서 카메라 위치 설정
            transform.position = player.position + offset;
        }
    }

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 desiredPosition = player.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
