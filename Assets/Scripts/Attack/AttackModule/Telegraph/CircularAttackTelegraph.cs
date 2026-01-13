
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Telegraph/CircularLine")]
public class CircularAttackTelegraph : TelegraphSO
{
    public Color color = Color.red;
    public float width = 0.25f;
    public float maxDistance = 50f;
    public float radius = 0.3f;
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

        // loop를 쓰면 마지막 점을 첫 점으로 자동 연결해줘서 positionCount = segments 로도 OK
        lr.loop = true;
        int segments = 32;
        Vector3 center = GameManager.instance.player.transform.position;

        int count = true ? segments : segments + 1;
        lr.positionCount = count;

        // "중점에서 반지름만큼 위로 올린 곳" (12시)부터 시작: 각도 90도
        float startAngleRad = Mathf.PI * 0.5f; // 90deg

        for (int i = 0; i < count; i++)
        {
            float t = (float)i / (segments); // loop면 마지막이 1.0 근처
            float angle = startAngleRad + t * Mathf.PI * 2f;

            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            Vector3 pos = new Vector3(center.x + x, center.y + y, center.z);
            lr.SetPosition(i, pos);
        }
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


