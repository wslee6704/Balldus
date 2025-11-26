using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArrowRenderer : MonoBehaviour
{
    private LineRenderer arrowLine = null;

    float headSize = 0.5f;
    float maxLength = 8f;

    private GameObject target;
    Action<Vector3>[] actions = new Action<Vector3>[2];
    private CircleCollider2D coll;

    [SerializeField]
    InputType type;
    ActionType actionType;

    void Awake()
    {
        arrowLine = GetComponent<LineRenderer>();
        this.target = transform.parent.gameObject;//자신을 사용하고 있는 부모를 따라간다
        actions[(int)ActionType.Movement] = (endPos) => target.GetComponent<MoveTest>().move(endPos);
        actions[(int)ActionType.Attack] = (pos) => Fire();
        coll = target.GetComponent<CircleCollider2D>();
    }

    void Fire()
    {
        Debug.Log("쏴라!");
    }
    //오브젝트를 추적하여 그릴 수 있게한다
    void Init(GameObject target)
    {
        this.target = transform.parent.gameObject;
        //maxLength = 플레이어의 이동거리
    }

    void Update()
    {
        if (type == InputType.Mouse) MouseCalculate();
        else KeyboardCalculate();
    }


    Vector3 mousePos;
    bool clickEnable = false;
    Vector3 endPos, startPointer, targetPos;
    float percentSize;//화살촉의 비율

    void MouseCalculate()//알까기 처럼 플레이어를 잡아당기는 것도 가능, 외부 공백에서 잡아당기는것도 가능
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            startPointer = mousePos;
            startPointer.z = 0f;
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null)
            {
                Debug.Log("충돌된 오브젝트: " + hit.collider.gameObject.name);

                if (hit.collider.gameObject == target)
                {
                    Debug.Log("이 오브젝트를 클릭했다!");
                    //clickEnable = true;
                    startPointer = target.transform.position;
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 pointer = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            MaxLengthCorrection(pointer);
            ShapeRenderHelper.DrawArrow(arrowLine, targetPos, endPos, percentSize);
            //ShapeRenderHelper.DrawSemiCircle(arrowLine,targetPos,endPos,5);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //clickEnable = false;
            arrowLine.positionCount = 0;
            actions[(int)actionType]?.Invoke(endPos);
        }
    }

    void MaxLengthCorrection(Vector3 pointer)
    {
        targetPos = target.transform.position;
        pointer.z = 0;

        Vector3 rawDir = pointer - startPointer;   // 사용자가 드래그한 방향
        float rawDist = rawDir.magnitude;          // 드래그 크기
        Vector3 dirNorm = rawDir.normalized;

        // 1) 유저 입력거리와 최대거리 중 작은 것을 선택
        float wantedDist = Mathf.Min(rawDist, maxLength);

        // 2) Collider.Cast()로 실제 이동가능한 거리 계산
        float safeDist = CastForDistance(-dirNorm, wantedDist);

        // 3) 최종 이동거리 보정 후 endPos 계산
        Vector3 finalDir = -dirNorm * safeDist;
        endPos = targetPos + finalDir;

        // 화살촉 비율 계산
        float curLength = Mathf.Min(maxLength, rawDist);
        percentSize = headSize / curLength;
    }


    RaycastHit2D[] castHits = new RaycastHit2D[3];   // 작은 배열이면 충분

    float CastForDistance(Vector2 dir, float maxDist)
    {
        int hitCount = coll.Cast(
            dir,            // 이동 방향 (normalized)
            castHits,
            maxDist         // 최대 이동거리
        );

        if (hitCount > 0)
        {
            Debug.Log("충돌");
            // ★ 여기서 distance는 “완전히 부딪히기 직전까지 이동 가능한 거리”
            //   CircleCast와 달리 radius 계산이 이미 적용된 정확한 거리!
            return castHits[0].distance;
        }

        // 충돌 없으면 그대로 maxDist
        return maxDist;
    }



    Vector3 dirPointer = Vector3.zero;
    float power = 10f;
    void KeyboardCalculate()
    {
        targetPos = target.transform.position;
        startPointer = target.transform.position;
        // W ↑
        if (Input.GetKey(KeyCode.W))
            dirPointer.y -= power * Time.deltaTime;

        // S ↓
        if (Input.GetKey(KeyCode.S))
            dirPointer.y += power * Time.deltaTime;

        // D →
        if (Input.GetKey(KeyCode.D))
            dirPointer.x -= power * Time.deltaTime;

        // A ←
        if (Input.GetKey(KeyCode.A))
            dirPointer.x += power * Time.deltaTime;

        // ★ 핵심: 원형 범위 제한 (반지름 maxRadius)
        dirPointer = Vector3.ClampMagnitude(dirPointer, maxLength);
        MaxLengthCorrection(targetPos + dirPointer);
        if (dirPointer != Vector3.zero)
            ShapeRenderHelper.DrawArrow(arrowLine, targetPos, endPos, percentSize);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            actions[(int)actionType]?.Invoke(endPos);
            arrowLine.positionCount = 0;
            dirPointer = Vector3.zero;

        }

    }



    private enum InputType//이거는 세팅 매니저한테 넘기는게 나을듯?
    {
        Mouse,
        Keyboard,
    }
    private enum ActionType
    {
        Movement,
        Attack
    }
}
