using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform destinationPortal; // 이동할 포탈 오브젝트의 위치
    private bool isPlayerInPortalRange = false; // 플레이어가 포탈 범위 안에 있는지 여부

    private void Update()
    {
        // 플레이어가 포탈 범위 안에 있을 때 E 키를 누르면 포탈 이동
        if (isPlayerInPortalRange && Input.GetKeyDown(KeyCode.E))
        {
            TeleportPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 포탈 범위에 들어왔음을 체크
            isPlayerInPortalRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 포탈 범위를 벗어났음을 체크
            isPlayerInPortalRange = false;
        }
    }

    private void TeleportPlayer()
    {
        // 플레이어의 위치를 다른 포탈의 위치로 변경
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && destinationPortal != null)
        {
            Vector3 newPosition = destinationPortal.position;
            newPosition.y -= 1; // y축으로 1만큼 아래로 이동
            player.transform.position = newPosition;
        }
    }
}
