using System;
using UnityEngine;

public class ArrowDrawer : MonoBehaviour
{
    LineRenderer arrowLine = null;
    Vector3 drawStartPos;
    Vector3 drawEndPos;
    private void Start()
    {
        arrowLine = GetComponent<LineRenderer>();
        arrowLine.startColor = Color.gray;
        arrowLine.endColor   = Color.gray;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            drawStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            drawStartPos.z = 0;
        }
        else if (Input.GetMouseButton(0))
        {
            drawEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            drawEndPos.z = 0;

            DrawArrow(drawEndPos);
        }
        else
        {
            arrowLine.positionCount = 0;
        }
    }
    public void DrawArrow(Vector3 pointer)
    {
        float arrowheadSize = 0.5f;
        float maxLength = 5;


        // 시작점: 이 스크립트가 붙은 오브젝트 위치
        Vector3 startPos = this.transform.position;

        //------------------------------
        // ① pointer를 maxLength 범위 내로 제한
        //------------------------------
        Vector3 dir = pointer - startPos;
        float dist = dir.magnitude;    // 실제 거리

        if (dist > maxLength)
        {
            dir = dir.normalized * maxLength;
        }

        Vector3 endPos = startPos + dir;   // 제한된 끝점
        //------------------------------


        // 화살촉 비율 계산 (길이가 짧아도 문제 없도록)
        float curLength = Mathf.Min(maxLength, dist);
        float percentSize = arrowheadSize / curLength;

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

}
