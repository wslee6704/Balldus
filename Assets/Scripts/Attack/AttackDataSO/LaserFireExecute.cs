using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Execute/LaserFire")]
public class LaserFireExecute : ExecuteSO
{
    public override void Execute(AttackInstance inst, float now)
    {
        // 예: 즉발 히트라면 여기서 inst.IsPlayerHit(now) 보고 데미지
        // 혹은 Projectile 생성이라면 inst.LockedDir로 발사
        inst.owner.ExecuteAttack(inst.def, inst.LockedDir);
    }
}
