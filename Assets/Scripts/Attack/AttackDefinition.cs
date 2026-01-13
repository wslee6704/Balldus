using UnityEngine;

[CreateAssetMenu(menuName = "Combat/AttackDefinition")]
public class AttackDefinition : ScriptableObject
{
    
    [Header("Projectile")]
    public GameObject projectile;

    [Header("Timings")]
    public float aimFollowDuration = 1.5f;   // 조준 추적 시간
    public float aimLockDuration = 0.5f;   // 조준 확립(고정) 시간
    public float windupDuration = 0.3f;   // 발사 직전 텀
    

    [Header("패리가능 시간(0일시, aimLockDuration~windupDuration끝까지)")]
    public float parryStartOffset;           // 0이면 조준 끝난 시간으로
    public float parryEndOffset;             // 0이면 자동 계산(=fireTime)

    [Header("회피 기동 가능 시간")]
    public float dodgeStartOffset;           // 0이면 조준 끝난 시간으로
    public float dodgeEndOffset;             // 0이면 자동 계산(=fireTime)

    [Header("Audio")]
    public AudioManager.Sfx soundType;

    [Header("Modules")]
    public TelegraphSO telegraph;
    //Definition의 데이터를 인스턴스화 하는 AttackInstance에서 
    public IThreatCheck.CheckType theratType;
    public IExecuteFire.CheckType fireType;
    public IParryCheckModule.ParryType parryType;

    // 발사 시점(상대 시간)
    public float FireOffset =>
        aimFollowDuration + aimLockDuration + windupDuration;

    public float ParryStartTime => aimFollowDuration + aimLockDuration;

    public float ParryEndTime
        => (parryEndOffset > 0f) ? parryEndOffset : FireOffset;

    public float DodgeStartTime => aimFollowDuration;

    public float DodgeEndTime
        => (parryEndOffset > 0f) ? parryEndOffset : FireOffset;
}
