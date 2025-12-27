using UnityEngine;

[CreateAssetMenu(menuName = "Combat/AttackDefinition")]
public class AttackDefinition : ScriptableObject
{
    [Header("Timings")]
    public float aimFollowDuration = 1.5f;   // 조준 추적 시간
    public float aimLockDuration = 0.5f;   // 조준 확립(고정) 시간
    public float windupDuration = 0.3f;   // 발사 직전 텀
    public float cooldown = 1.0f;   // 다음 공격까지

    [Header("Parry Window (relative to start)")]
    public float parryStartOffset;           // 0이면 조준 끝난 시간으로
    public float parryEndOffset;             // 0이면 자동 계산(=fireTime)


    [Header("Modules")]
    public TelegraphSO telegraph;
    public ThreatTestSO threatTest;
    public ExecuteSO executor;

    // 발사 시점(상대 시간)
    public float FireOffset =>
        aimFollowDuration + aimLockDuration + windupDuration;

    public float ParryStartTime => aimFollowDuration;

    public float ParryEndTime
        => (parryEndOffset > 0f) ? parryEndOffset : FireOffset;
}
