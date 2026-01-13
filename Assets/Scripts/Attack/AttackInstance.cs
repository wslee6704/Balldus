using UnityEngine;

public enum AttackState { AimFollow, AimLock, Windup, Fired, Done, Canceled }

public class AttackInstance
{
    public readonly EnemyAttackPerformer owner;
    public readonly AttackDefinition def;
    public LineRenderer lr;
    public GameObject projectile;
    public Transform runtimeProjectileTr { get; set; }

    public AttackState State { get; private set; }

    public float StartTime { get; private set; }
    public Vector2 LockedDir { get; private set; }

    private float aimFollowEnd, aimLockEnd, fireTime, doneTime;

    IThreatCheck threatModule;
    IExecuteFire fireModule;
    IParryCheckModule parryCheckModule;

    public AttackInstance(EnemyAttackPerformer owner, AttackDefinition def)
    {
        this.owner = owner;
        this.def = def;
        projectile = def.projectile;
        runtimeProjectileTr =null;
        ModuleInit();
    }

    void ModuleInit()
    {
        //threaMoudule초기화
        switch (def.theratType)
        {
            case IThreatCheck.CheckType.Laser:
                threatModule = new RaycastLineThreat();
                break;
            default:
                break;
        }
        switch (def.fireType)
        {
            case IExecuteFire.CheckType.Laser:
                fireModule = new LaserFireExecute();
                break;
            case IExecuteFire.CheckType.Bullet:
                fireModule = new BulletFireExecute();
                break;
            default:
                break;
        }
        switch (def.parryType)
        {
            case IParryCheckModule.ParryType.Distance:
                parryCheckModule = new ParriableByDistance();
                break;
            case IParryCheckModule.ParryType.Timing:
                parryCheckModule = new ParriableByTime();
                break;
            default:
                break;
        }
    }


    public void Start(float now)
    {
        Debug.Log("인스턴스 초기화");
        lr = LinePoolManager.I.Rent(owner.transform);
        StartTime = now;
        aimFollowEnd = StartTime + def.aimFollowDuration;
        aimLockEnd = aimFollowEnd + def.aimLockDuration;
        fireTime = StartTime + def.FireOffset;
        doneTime = fireTime + 1f;

        State = AttackState.AimFollow;

        def.telegraph?.OnStart(this);
    }

    public void Tick(float now)
    {
        if (State is AttackState.Done or AttackState.Canceled) return;
        if (owner == null || !owner.IsAlive) { Cancel(); return; }

        switch (State)
        {
            case AttackState.AimFollow:
                LockedDir = owner.CalcAimDir();
                def.telegraph?.OnTick(this, now);
                if (now >= aimFollowEnd)
                {
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.Reload);
                    State = AttackState.AimLock;
                } 
                break;

            case AttackState.AimLock:
                def.telegraph?.OnTick(this, now);
                if (now >= aimLockEnd)
                {
                    State = AttackState.Windup;
                    
                } 
                break;

            case AttackState.Windup:
                def.telegraph?.OnTick(this, now);
                if (now >= fireTime)
                {
                    AudioManager.instance.PlaySfx(def.soundType);
                    fireModule?.Execute(this, now);
                    State = AttackState.Fired;
                }
                break;

            case AttackState.Fired:
                if (now >= doneTime) State = AttackState.Done;
                break;
        }
    }

    public bool IsParryable(float now)
    { 
        return parryCheckModule.IsParryable(this, now);
    }

    public bool IsDodgeable(float now)
    {
        float local = now - StartTime;
        return local >= def.DodgeStartTime && local < def.DodgeEndTime;
    }

    public bool IsThreateningNow(float now)
        => threatModule != null && threatModule.IsThreateningNow(this, now);

    public bool IsPlayerHit(float now)
        => threatModule != null && threatModule.IsPlayerHit(this, now);

    public void Cancel()
    {
        State = AttackState.Canceled;
        fireModule?.CancelAttack();
        def.telegraph?.OnClear(this);
    }

    public void Dispose()
    {
        def.telegraph?.OnClear(this);
    }

    public void CallDamageEvent()//총알류가 충돌되었을때 호출
    {
        Debug.Log("이벤트 호출 플레이어가 충돌됨");
    }

}
