using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArrowRenderer : MonoBehaviour
{
    private LineRenderer arrowLine = null;

    float headSize = 0.5f;
    float maxLength = 5f;

    private GameObject target;

    [SerializeField]
    InputType type;

    void Awake()
    {
        arrowLine = GetComponent<LineRenderer>();
        this.target = transform.parent.gameObject;//자신을 사용하고 있는 부모를 따라간다
    }
    //오브젝트를 추적하여 그릴 수 있게한다
    void Init(GameObject target)
    {
        this.target = transform.parent.gameObject;
    }

    void FixedUpdate()
    {
        ///
        /// 테스트용 코드
        /// 마우스 입력방식과, 키보드 입력방식을 구분을 둔다
        /// 
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     if (type == InputType.Mouse) type = InputType.Keyboard;
        //     else type = InputType.Mouse;
        // }

        if (type == InputType.Mouse) MouseCalculate();
        else KeyboardCalculate();
    }


    Vector3 mousePos;
    bool clickEnable = false;
    Vector3 endPos, startPos, targetPos;
    float percentSize;//화살촉의 비율

    void MouseCalculate()//알까기 처럼 플레이어를 잡아당기는 것도 가능, 외부 공백에서 잡아당기는것도 가능
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            startPos = mousePos;
            startPos.z = 0f;
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null)
            {
                Debug.Log("충돌된 오브젝트: " + hit.collider.gameObject.name);

                if (hit.collider.gameObject == target)
                {
                    Debug.Log("이 오브젝트를 클릭했다!");
                    //clickEnable = true;
                    startPos = target.transform.position;
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 pointer = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            MaxLengthCorrection(pointer);
            DrawArrow();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //clickEnable = false;
            arrowLine.positionCount = 0;
            target.GetComponent<MoveTest>().move(endPos);
        }
    }

    void MaxLengthCorrection(Vector3 pointer)//최대 길이 보정함수 
    {
        targetPos = target.transform.position;
        //카메라가 10f 떨어져있어서 초기화를 해준후 dist를 계산해야함
        pointer.z = 0f;
        Vector3 dir = pointer - startPos;
        float dist = dir.magnitude;    // 실제 거리

        if (dist > maxLength)
        {
            dir = dir.normalized * maxLength;
        }
        endPos = targetPos + (-dir);//알까기처럼 반대방향으로 해주기 위해 -를 해줌
        //-dir만으로 계산을 한다면 z값에 오류가 난다고 한다
        // 화살촉 비율 계산 (길이가 짧아도 문제 없도록)
        float curLength = Mathf.Min(maxLength, dist);
        percentSize = headSize / curLength;
    }

    void DrawArrow()
    {

        // LineRenderer 설정
        arrowLine.positionCount = 4;

        // 0: 시작점
        arrowLine.SetPosition(0, targetPos);

        // 1: 몸통 끝 (화살촉 직전)
        arrowLine.SetPosition(1, Vector3.Lerp(targetPos, endPos, 0.999f - percentSize));

        // 2: 화살촉 시작점
        arrowLine.SetPosition(2, Vector3.Lerp(targetPos, endPos, 1 - percentSize));

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

    Vector3 dirPointer = Vector3.zero;
    float power = 3f;
    void KeyboardCalculate()
    {
        targetPos = target.transform.position;
        // W ↑
        if (Input.GetKey(KeyCode.W))
            dirPointer.y -= power * Time.fixedDeltaTime;

        // S ↓
        if (Input.GetKey(KeyCode.S))
            dirPointer.y += power * Time.fixedDeltaTime;

        // D →
        if (Input.GetKey(KeyCode.D))
            dirPointer.x -= power * Time.fixedDeltaTime;

        // A ←
        if (Input.GetKey(KeyCode.A))
            dirPointer.x += power * Time.fixedDeltaTime;

        MaxLengthCorrection(targetPos + dirPointer);
        if (dirPointer != Vector3.zero)
            DrawArrow();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            arrowLine.positionCount = 0;
            dirPointer = Vector3.zero;
            target.GetComponent<MoveTest>().move(endPos);
        }

    }



    private enum InputType//이거는 세팅 매니저한테 넘기는게 나을듯?
    {
        Mouse,
        Keyboard,
    }
}
