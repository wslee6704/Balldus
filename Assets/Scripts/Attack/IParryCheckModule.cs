using UnityEngine;

public interface IParryCheckModule
{
    public enum ParryType{Timing, Distance}
    public bool IsParryable(AttackInstance inst, float now);


}

public class ParriableByDistance : MonoBehaviour, IParryCheckModule
{
    public bool IsParryable(AttackInstance inst, float now)
    {

        var proj = inst.runtimeProjectileTr;
        if (!proj) return false;
        float dist = Vector2.Distance((Vector2)GameManager.instance.player.transform.position, 
        proj.position);
         Debug.Log($"거리 패링,  {dist}");
        return dist >= inst.def.parryStartOffset && dist < inst.def.ParryEndTime;
    }
}

public class ParriableByTime : MonoBehaviour, IParryCheckModule
{
    public bool IsParryable(AttackInstance inst, float now)
    {
        
        float local = now - inst.StartTime;
        Debug.Log($"로컬 시간{local}, 패리 시작 시간 {inst.def.ParryStartTime} 패리 종료 시간 {inst.def.ParryEndTime}");
        bool result = local >= inst.def.ParryStartTime && local < inst.def.ParryEndTime;
        Debug.Log($"패리가능시간 : {result}");
        return result;
    }
}