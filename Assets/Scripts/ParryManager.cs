using System.Collections.Generic;
using UnityEngine;

public class ParryManager : MonoBehaviour
{
    public static ParryManager Instance;

    private List<EnemyAttackInfo> activeAttacks = new List<EnemyAttackInfo>();

    void Awake() => Instance = this;

    public void RegisterAttack(EnemyAttackInfo info)
    {
        activeAttacks.Add(info);
    }

    void Update()
    {
        float t = Time.time;

        // 시간이 지나서 끝난 공격은 제거
        activeAttacks.RemoveAll(a => t > a.hitTime);
    }

    public EnemyAttackInfo GetCurrentParryableAttack()
    {
        float t = Time.time;

        foreach (var a in activeAttacks)
        {
            if (a.IsParryable(t)&& a.owner.CanHitPlayer())
            {
                return a;
            }
                
        }

        return null; // 지금 패링 가능한 공격 없음
    }
}
