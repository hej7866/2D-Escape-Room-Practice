using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Enemy 태그와 충돌하면 제거
        if (!collision.CompareTag("Player"))
        {
            Destroy(gameObject); // 총알 제거
        }
    }
}
