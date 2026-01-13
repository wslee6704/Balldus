using System.Collections.Generic;
using UnityEngine;

public class ParryManager : MonoBehaviour
{
    public static ParryManager I { get; private set; }

    private readonly List<AttackInstance> active = new();

    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
    }

    public AttackInstance StartAttack(EnemyAttackPerformer owner, AttackDefinition def)
    {
        //AudioManager.instance.PlaySfx(AudioManager.Sfx.Reload);
        var inst = new AttackInstance(owner, def);
        inst.Start(Time.time);
        active.Add(inst);
        return inst;
    }

    void Update()
    {
        float now = Time.time;

        for (int i = active.Count - 1; i >= 0; i--)
        {
            var a = active[i];
            a.Tick(now);

            if (a.State == AttackState.Done || a.State == AttackState.Canceled)
            {
                Debug.Log($"{a.State}로 반납됨");
                a.Dispose();
                active.RemoveAt(i);
            }
        }
    }

    // 플레이어가 패리 버튼을 누르면 호출
    public bool TryParry()
    {
        float now = Time.time;

        // 선택 규칙: “패리 가능 + 위협 중”인 것 중 하나
        for (int i = 0; i < active.Count; i++)
        {
            var a = active[i];
            if ( a.IsThreateningNow(now)&&a.IsParryable(now) )
            {
                a.Cancel();
                return true;
            }
        }
        return false;
    }
}
