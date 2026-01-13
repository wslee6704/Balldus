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

    // “진짜 공격”은 여기서. (함수 기반 처리/총알 생성/즉발 판정 등)
    public void ExecuteAttack(AttackDefinition def, Vector2 lockedDir)
    {
        Debug.Log($"{this}의 공격 함수 실행!");
    }

    // 예시: 죽으면 매니저가 자동 취소하도록
    public void Kill() => IsAlive = false;
}
