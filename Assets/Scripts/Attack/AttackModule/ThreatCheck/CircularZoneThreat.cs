using UnityEngine;

public class CircularZoneThreat : MonoBehaviour, IThreatCheck
{
    public bool IsThreateningNow(AttackInstance inst, float now)
    {
        return true;
    }

    public bool IsPlayerHit(AttackInstance inst, float now)
    {
        return true;
    }
}
