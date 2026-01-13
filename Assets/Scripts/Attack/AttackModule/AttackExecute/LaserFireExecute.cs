using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LaserFireExecute : MonoBehaviour, IExecuteFire
{
    AttackInstance inst;
    public void Execute(AttackInstance inst, float now)
    {
        this.inst = inst;
        if (inst.IsThreateningNow(now))
        {
            CallInstanceEvent();
        }
        
        // 예: 즉발 히트라면 여기서 inst.IsPlayerHit(now) 보고 데미지
        // 혹은 Projectile 생성이라면 inst.LockedDir로 발사
        //inst.owner.ExecuteAttack(inst.def, inst.LockedDir);
    }

    public void CallInstanceEvent()
    {
        inst.CallDamageEvent();
    }

    public void CancelAttack()
    {
        
    }
}
