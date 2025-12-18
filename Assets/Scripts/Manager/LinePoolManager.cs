using System.Collections.Generic;
using UnityEngine;

public class LinePoolManager : MonoBehaviour
{
    public static LinePoolManager I { get; private set; }

    [Header("Prefab (must have LineRenderer)")]
    [SerializeField] private LineRenderer linePrefab;

    [Header("Pool Settings")]
    [SerializeField] private int prewarmCount = 16;
    [SerializeField] private Transform poolRoot; // optional: lines parent

    // 대기(미사용) 목록
    private readonly Stack<LineRenderer> free = new Stack<LineRenderer>(128);
    // 사용 중인지 추적 (중복 반납/외부 파괴 방지용)
    private readonly HashSet<LineRenderer> inUse = new HashSet<LineRenderer>();

    void Awake()
    {
        if (I != null && I != this)
        {
            Destroy(gameObject);
            return;
        }
        I = this;

        if (poolRoot == null) poolRoot = transform;

        Prewarm(prewarmCount);
    }

    void Prewarm(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var lr = CreateNew();
            free.Push(lr);
        }
    }

    LineRenderer CreateNew()
    {
        var lr = Instantiate(linePrefab, poolRoot);
        lr.gameObject.SetActive(false);

        // 안전 초기값
        lr.positionCount = 0;
        lr.useWorldSpace = true; // 보통 조준선은 월드 기준이 편함 (원하면 프리팹에서 설정)
        return lr;
    }

    /// <summary>
    /// 라인을 하나 빌려온다.
    /// owner를 넘기면 계층 정리(누가 쓰는 라인인지)와 추적이 편해진다.
    /// </summary>
    public LineRenderer Rent(Transform owner = null)
    {
        LineRenderer lr = (free.Count > 0) ? free.Pop() : CreateNew();

        inUse.Add(lr);

        if (owner != null)
            lr.transform.SetParent(owner, worldPositionStays: false);
        else
            lr.transform.SetParent(poolRoot, worldPositionStays: false);

        // 렌탈 초기화
        lr.positionCount = 0;        // 이전 점 남는 것 방지
        lr.widthCurve = AnimationCurve.Linear(0, 1, 1, 1); // 기본으로 리셋(원하면 제거 가능)
        lr.widthMultiplier = 1f;

        lr.gameObject.SetActive(true);
        return lr;
    }

    /// <summary>
    /// 라인을 반납한다.
    /// </summary>
    public void Return(LineRenderer lr)
    {
        if (lr == null) return;

        // 이미 반납된 것, 혹은 외부에서 만든 것 방지
        if (!inUse.Remove(lr))
        {
            // Debug.LogWarning("Tried to return a LineRenderer not in use.");
            return;
        }

        // 비활성화 + 정리
        lr.positionCount = 0;
        lr.gameObject.SetActive(false);
        lr.transform.SetParent(poolRoot, worldPositionStays: false);

        free.Push(lr);
    }

    /// <summary>
    /// owner가 빌린 라인을 한 번에 모두 정리하고 싶을 때(선택).
    /// owner 아래의 LineRenderer 전부 Return.
    /// </summary>
    public void ReturnAllUnder(Transform owner)
    {
        if (owner == null) return;

        // owner 아래 자식 중 LineRenderer만 수집 후 반납
        // (반납 중 부모가 바뀌므로 미리 배열로 복사)
        var lrs = owner.GetComponentsInChildren<LineRenderer>(includeInactive: true);
        foreach (var lr in lrs)
        {
            // 풀에서 빌린 것만 반납되도록 inUse 체크가 걸러줌
            Return(lr);
        }
    }
}
