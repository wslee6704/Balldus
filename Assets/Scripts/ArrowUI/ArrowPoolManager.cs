using System.Collections.Generic;
using UnityEngine;

public class ArrowPoolManager : MonoBehaviour
{
    public static ArrowPoolManager Instance;

    [Header("Pool Settings")]
    public LineRenderer arrowPrefab;
    public int initialPoolSize = 10;

    private readonly Queue<LineRenderer> pool = new Queue<LineRenderer>();

    void Awake()
    {
        Instance = this;
        InitPool();
    }

    void InitPool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewArrow();
        }
    }

    LineRenderer CreateNewArrow()
    {
        LineRenderer newArrow = Instantiate(arrowPrefab, transform);
        newArrow.gameObject.SetActive(false);
        pool.Enqueue(newArrow);
        return newArrow;
    }

    // Arrow 빌려오기
    public LineRenderer RentArrow()
    {
        if (pool.Count > 0)
        {
            var arrow = pool.Dequeue();
            arrow.gameObject.SetActive(true);
            return arrow;
        }
        return CreateNewArrow();
    }

    // Arrow 반환
    public void ReturnArrow(LineRenderer arrow)
    {
        arrow.gameObject.SetActive(false);
        pool.Enqueue(arrow);
    }
}
