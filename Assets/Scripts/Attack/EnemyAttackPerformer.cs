using UnityEngine;

public class EnemyAttackPerformer : MonoBehaviour
{
    [SerializeField] private Transform player;
    public bool IsAlive { get; private set; } = true;

    public Vector3 AimOrigin => transform.position;

    // 조준 중엔 플레이어를 계속 추적할 때 호출
    public Vector2 CalcAimDir()
    {
        if (!player) return Vector2.right;
        return ((Vector2)(player.position - transform.position)).normalized;
    }
    //벽면같은 장애물에 닿는곳까지 에임 확정 짓게하기.
    // public Vector3 CalcAimEnd(AttackDefinition def, Vector2 dir)
    // {
    //     Vector2 origin = transform.position;
    //     var hit = Physics2D.Raycast(origin, dir, def.maxDistance, def.wallMask);
    //     return hit.collider ? (Vector3)hit.point : (Vector3)(origin + dir * def.maxDistance);
    // }

    // public bool CanHitPlayer(AttackDefinition def, Vector2 dir)
    // {
    //     // 벽 마스크를 빼고 플레이어까지 쏘고 싶다면 레이어/마스크를 따로 관리하는 게 좋음.
    //     var hit = Physics2D.Raycast(transform.position, dir, def.maxDistance);
    //     return hit.collider != null && hit.collider.CompareTag("Player");
    // }

    // “진짜 공격”은 여기서. (함수 기반 처리/총알 생성/즉발 판정 등)
    public void ExecuteAttack(AttackDefinition def, Vector2 lockedDir)
    {
        if (CanHitPlayer(def, lockedDir)) Debug.Log($"{name}: 플레이어 공격 성공!");
        else Debug.Log($"{name}: 회피!");
    }

    // 예시: 죽으면 매니저가 자동 취소하도록
    public void Kill() => IsAlive = false;
}
