using System.Collections.Generic;
using UnityEngine;

public class LinePoolManager : MonoBehaviour
{
    public static LinePoolManager I { get; private set; }

    [SerializeField] private LineRenderer linePrefab;
    [SerializeField] private int prewarm = 16;
    [SerializeField] private Transform poolRoot;

    private readonly Stack<LineRenderer> free = new();
    private readonly HashSet<LineRenderer> inUse = new();

    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
        if (!poolRoot) poolRoot = transform;

        for (int i = 0; i < prewarm; i++) free.Push(CreateNew());
    }

    LineRenderer CreateNew()
    {
        var lr = Instantiate(linePrefab, poolRoot);
        lr.gameObject.SetActive(false);
        lr.positionCount = 0;
        lr.useWorldSpace = true;
        return lr;
    }

    public LineRenderer Rent(Transform owner = null)
    {
        Debug.Log("라인 대여");
        var lr = free.Count > 0 ? free.Pop() : CreateNew();
        inUse.Add(lr);

        //lr.transform.SetParent(owner ? owner : poolRoot, false);
        lr.positionCount = 0;
        lr.gameObject.SetActive(true);
        return lr;
    }

    public void Return(LineRenderer lr)
    {
        if (!lr) return;
        if (!inUse.Remove(lr)) return;

        lr.positionCount = 0;
        lr.gameObject.SetActive(false);
        //lr.transform.SetParent(poolRoot, false);
        free.Push(lr);
    }
}
