using UnityEngine;

public enum AttackState { AimFollow, AimLock, Windup, Fired, Done, Canceled }

public class AttackInstance
{
    public readonly EnemyAttackPerformer owner;
    public readonly AttackDefinition def;

    public AttackState State { get; private set; }

    public float StartTime { get; private set; }
    public Vector2 LockedDir { get; private set; }

    private float aimFollowEnd, aimLockEnd, fireTime, doneTime;

    public AttackInstance(EnemyAttackPerformer owner, AttackDefinition def)
    {
        this.owner = owner;
        this.def = def;
    }

    public void Start(float now)
    {
        StartTime = now;
        aimFollowEnd = StartTime + def.aimFollowDuration;
        aimLockEnd = aimFollowEnd + def.aimLockDuration;
        fireTime = StartTime + def.FireOffset;
        doneTime = fireTime + def.cooldown;

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
                if (now >= aimFollowEnd) State = AttackState.AimLock;
                break;

            case AttackState.AimLock:
                def.telegraph?.OnTick(this, now);
                if (now >= aimLockEnd) State = AttackState.Windup;
                break;

            case AttackState.Windup:
                def.telegraph?.OnTick(this, now);
                if (now >= fireTime)
                {
                    def.executor?.Execute(this, now);
                    def.telegraph?.OnClear(this);
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
        if (State != AttackState.Windup && State != AttackState.AimLock) return false;
        float local = now - StartTime;
        return local >= def.ParryStartTime && local < def.ParryEndTime;
    }

    public bool IsThreateningNow(float now)
        => def.threatTest != null && def.threatTest.IsThreateningNow(this, now);

    public bool IsPlayerHit(float now)
        => def.threatTest != null && def.threatTest.IsPlayerHit(this, now);

    public void Cancel()
    {
        State = AttackState.Canceled;
        def.telegraph?.OnClear(this);
    }

    public void Dispose()
    {
        def.telegraph?.OnClear(this);
    }
}
