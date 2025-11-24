using UnityEngine;

public class ShapeRenderHelper : MonoBehaviour
{
    public static void DrawArrow(LineRenderer arrowLine, Vector3 startPos, Vector3 endPos, float percentSize)
    {

        // LineRenderer 설정
        arrowLine.positionCount = 4;

        // 0: 시작점
        arrowLine.SetPosition(0, startPos);

        // 1: 몸통 끝 (화살촉 직전)
        arrowLine.SetPosition(1, Vector3.Lerp(startPos, endPos, 0.999f - percentSize));

        // 2: 화살촉 시작점
        arrowLine.SetPosition(2, Vector3.Lerp(startPos, endPos, 1 - percentSize));

        // 3: 화살촉 끝점
        arrowLine.SetPosition(3, endPos);

        // 화살 굵기 커브
        arrowLine.widthCurve = new AnimationCurve(
            new Keyframe(0, 0.2f),
            new Keyframe(0.999f - percentSize, 0.2f),
            new Keyframe(1 - percentSize, 0.5f),
            new Keyframe(1 - percentSize, 0.5f),
            new Keyframe(1, 0f)
        );
    }


    public static void DrawSemiCircle(LineRenderer lr, Vector3 startPos, Vector3 endPos, float radius)
    {
        lr.widthMultiplier = 0.3f;
        int segments = 40;
        lr.positionCount = segments + 4;
        Vector3 dir = endPos - startPos;
        float centerAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        int index = 0;

        // 1) 중심점
        lr.SetPosition(index++, startPos);

        // 2) 시작각(첫 포인트)
        {
            float rad = (centerAngle - 90f) * Mathf.Deg2Rad;
            Vector3 p = startPos + new Vector3(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;
            lr.SetPosition(index++, p);
        }

        for (int i = 0; i <= segments; i++)
        {
            float t = (float)i / segments;
            float angle = Mathf.Lerp(centerAngle - 90, centerAngle + 90, t) * Mathf.Deg2Rad;



            Vector3 p = startPos + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            lr.SetPosition(index++, p);
        }

        // 4) 끝각 → 중심 복귀
        lr.SetPosition(index, startPos);
    }
}
