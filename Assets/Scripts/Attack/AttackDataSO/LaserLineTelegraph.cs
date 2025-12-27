using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Telegraph/LaserLine")]
public class LaserLineTelegraph : TelegraphSO
{
    public Color color = Color.red;
    public float width = 0.25f;
    public float maxDistance = 50f;
    public LayerMask wallMask;

    private LineRenderer lr;

    public override void OnStart(AttackInstance inst)
    {
        lr = LinePoolManager.I.Rent(inst.owner.transform);
        lr.startColor = color;
        lr.endColor = color;
        lr.widthMultiplier = width;
        lr.positionCount = 0;
    }

    public override void OnTick(AttackInstance inst, float now)
    {
        if (!lr) return;

        Vector2 origin = inst.owner.AimOrigin;
        Vector2 dir = inst.LockedDir;

        RaycastHit2D hit = Physics2D.Raycast(origin, dir, maxDistance, wallMask);
        Vector3 end = hit.collider ? (Vector3)hit.point : (Vector3)(origin + dir * maxDistance);

        lr.positionCount = 2;
        lr.SetPosition(0, origin);
        lr.SetPosition(1, end);
    }

    public override void OnClear(AttackInstance inst)
    {
        if (!lr) return;
        lr.positionCount = 0;
        LinePoolManager.I.Return(lr);
        lr = null;
    }
}
