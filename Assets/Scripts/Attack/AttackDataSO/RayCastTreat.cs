using UnityEngine;

[CreateAssetMenu(menuName = "Combat/ThreatTest/RaycastLine")]
public class RaycastLineThreat : ThreatTestSO
{
    public float maxDistance = 50f;
    public LayerMask hitMask; // 플레이어/장애물 포함 정책에 맞게

    public override bool IsThreateningNow(AttackInstance inst, float now)
    {
        var o = (Vector2)inst.owner.AimOrigin;
        var d = inst.LockedDir;
        var hit = Physics2D.Raycast(o, d, maxDistance, hitMask);
        return hit.collider != null && hit.collider.CompareTag("Player");
    }

    public override bool IsPlayerHit(AttackInstance inst, float now)
    {
        // Fired 시점에 맞았는지 체크를 여기서 통일
        return IsThreateningNow(inst, now);
    }
}
