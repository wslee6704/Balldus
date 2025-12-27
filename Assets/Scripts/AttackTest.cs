using UnityEngine;

public class AttackTest : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [Header("Raycast")]
    [SerializeField] private float maxDistance = 50f;
    [SerializeField] private LayerMask wallMask;

    // 현재 조준 방향(매니저가 Aiming에서 계속 갱신)
    private Vector2 dir;

    public Vector3 AimOrigin => transform.position;

    public Vector2 CalcAimDir()
    {
        dir = (player.transform.position - transform.position).normalized;
        return dir;
    }

    public Vector3 CalcAimEnd(Vector2 aimDir)
    {
        Vector2 origin = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(origin, aimDir, maxDistance, wallMask);
        return hit.collider ? (Vector3)hit.point : (Vector3)(origin + aimDir * maxDistance);
    }

    public bool CanHitPlayer(Vector2 aimDir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, aimDir, maxDistance);
        return hit.collider != null && hit.collider.CompareTag("Player");
    }

    public void AttackPlayer(Vector2 aimDir)
    {
        if (CanHitPlayer(aimDir)) Debug.Log("플레이어 공격성공!");
        else Debug.Log("회피!");
    }
}


