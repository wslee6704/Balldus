using UnityEngine;


public class RaycastLineThreat : MonoBehaviour, IThreatCheck
{
    public float maxDistance = 50f;
    //public LayerMask hitMask = ; // 플레이어/장애물 포함 정책에 맞게

    public bool IsThreateningNow(AttackInstance inst, float now)
    {
        var o = (Vector2)inst.owner.AimOrigin;
        var d = inst.LockedDir;
        var hit = Physics2D.Raycast(o, d, maxDistance, LayerMask.GetMask("Platform"));
        //if(hit.collider != null) Debug.Log(hit.collider.tag);
        bool result = hit.collider != null && hit.collider.CompareTag("Player");
        Debug.Log($"레이저 조준 유무 {result}");
        return result;
    }

    public bool IsPlayerHit(AttackInstance inst, float now)
    {
        // Fired 시점에 맞았는지 체크를 여기서 통일
        return IsThreateningNow(inst, now);
    }
}
