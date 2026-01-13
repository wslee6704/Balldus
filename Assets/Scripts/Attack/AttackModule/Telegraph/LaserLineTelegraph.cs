using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Telegraph/LaserLine")]
public class LaserLineTelegraph : TelegraphSO
{
    public Color color = Color.red;
    public float width = 0.25f;
    public float maxDistance = 50f;
    public LayerMask wallMask;

    public override void OnStart(AttackInstance inst)
    {
        var lr = inst.lr;
        if (!lr) return;

        lr.startColor = color;
        lr.endColor = color;
        lr.widthMultiplier = width;
        lr.positionCount = 0;
    }

    public override void OnTick(AttackInstance inst, float now)
    {
        var lr = inst.lr;
        if (!lr) return;

        Vector2 origin = inst.owner.AimOrigin;
        Vector2 dir = inst.LockedDir;

        RaycastHit2D hit = Physics2D.Raycast(origin, dir, maxDistance, wallMask);

        float offset = 1.3f; // ← 벽에서 살짝 띄우고 싶다면 이 값 조절

        Vector3 end;
        if (hit.collider)
        {
            end = hit.point + dir * offset;
        }
        else
        {
            end = origin + dir * maxDistance;
        }

        lr.positionCount = 2;
        lr.SetPosition(0, origin);
        lr.SetPosition(1, end);
    }


    public override void OnClear(AttackInstance inst)
    {
        var lr = inst.lr;
        if (!lr) return;

        lr.positionCount = 0;
        LinePoolManager.I.Return(lr);
        inst.lr = null; // ✅ 소유권 정리
    }
}
