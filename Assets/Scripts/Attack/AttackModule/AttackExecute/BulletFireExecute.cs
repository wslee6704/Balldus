using UnityEngine;

public class BulletFireExecute : MonoBehaviour, IExecuteFire
{
    AttackInstance inst;
    RangeBullet bulletFunc;
    public void Execute(AttackInstance inst, float now)
    {
        
        Vector3 dir = (Vector3)inst.LockedDir;
        float speed = 30f;
        this.inst = inst;

        GameObject bullet = Instantiate(inst.projectile);
        inst.runtimeProjectileTr = bullet.transform;
        bullet.transform.position = inst.owner.transform.position;
        bulletFunc = bullet.GetComponent<RangeBullet>();
        bulletFunc.Init(dir,speed,this);
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
        bulletFunc.DisposeBullet();
    }
}

//여기서 음.,.,. 총알 받고, 위치를 받아와야함.
//인스턴스가 아닌 해당 모듈에서 위기를 체크한다?
//위협체크를 총알을 "발사"만 하게 하는게 좋은가
//인스턴스에서 실제로 맞았는지 체크를 해줘야할거같은데
