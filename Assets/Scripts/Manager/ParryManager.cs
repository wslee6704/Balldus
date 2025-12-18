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

    //패링과 무빙을 칠 때, 자신을 조준하는 공격이 있는지 확인
    //패링 가능 시간에 움직임을 하였고, 자신을 조준 성공한 공격이 있을때 움직이면 패링 가능한 공격을 return하고 제거할듯
    public EnemyAttackInfo GetCurrentParryableAttack()
    {
        float t = Time.time;

        foreach (var a in activeAttacks)
        {
            if (a.IsParryable(t) && a.owner.CanHitPlayer())
            {
                return a;
            }

        }

        return null; // 지금 패링 가능한 공격 없음
    }

    private void AttackByInfo()
    {

        foreach (var a in activeAttacks)
        {

        }
    }
}
