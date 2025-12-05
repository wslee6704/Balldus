using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    public PlayerState currentState { get; private set; }
    void Awake()
    {
        currentState = PlayerState.Idle;
    }
    public void ChangeState(PlayerState nextState)
    {
        if(currentState == nextState) return;

        currentState = nextState;
    }
}
public enum PlayerState
{
    Idle,
    Entering
}
