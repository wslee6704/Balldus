using UnityEngine;

public abstract class TelegraphSO : ScriptableObject
//조준선, 피격판정등을 그려주기 위한 함수
{
    public abstract void OnStart(AttackInstance inst); // 라인 풀 빌리기 등
    public abstract void OnTick(AttackInstance inst, float now); // 매 프레임 갱신
    public abstract void OnClear(AttackInstance inst); // 반납
}

public interface IThreatCheck
{
    public enum CheckType{Laser};
    // "지금 이 공격이 실제로 위협적인가?" (레이저면 Raycast, 휘두르기면 Overlap 등)
    public bool IsThreateningNow(AttackInstance inst, float now);

    // "이 공격에 플레이어가 맞는가?" (발사 시점/즉발 시점에 호출)
    public bool IsPlayerHit(AttackInstance inst, float now);
}

public interface IExecuteFire
{
    public enum CheckType{Laser, Bullet};
    // Fired 시점에 호출
    public void Execute(AttackInstance inst, float now);

    public void CallInstanceEvent();
    
    public void CancelAttack();
}