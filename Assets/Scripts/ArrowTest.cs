using UnityEngine;

public class ArrowDrawer : MonoBehaviour
{
    LineRenderer arrowLine = null;
    Vector3 drawStartPos;
    Vector3 drawEndPos;
    private void Start()
    {
        arrowLine = GetComponent<LineRenderer>();
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
    }
    public void DrawArrow(Vector3 pointer)
    {
        float arrowheadSize = 1;
        pointer.y = drawStartPos.y;
        //시작점과 끝점을 기준으로, 길이가 길어져도 헤드 사이즈는 똑같게 나오게 사용
        float percentSize = (float)(arrowheadSize / Vector3.Distance(drawStartPos, pointer));
        arrowLine.positionCount = 4;
        //0 시작 1 몸통 끝 2 화살촉 시작 3 화살촉 끝
        arrowLine.SetPosition(0, drawStartPos);
        arrowLine.SetPosition(1, Vector3.Lerp(drawStartPos, pointer, 0.999f - percentSize));
        arrowLine.SetPosition(2, Vector3.Lerp(drawStartPos, pointer, 1 - percentSize));
        arrowLine.SetPosition(3, pointer);
        arrowLine.widthCurve = new AnimationCurve(
        new Keyframe(0, 0.4f),
        new Keyframe(0.999f - percentSize, 0.4f),
        new Keyframe(1 - percentSize, 1f),
        new Keyframe(1 - percentSize, 1f),
        new Keyframe(1, 0f));
    }
}
