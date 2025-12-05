using UnityEngine;

public class ParryTest : MonoBehaviour
{
    PlayerStateController stateController;
    void Awake()
    {
        stateController = GetComponent<PlayerStateController>();
    }

    void Update()
    {
        //방향을 재고 있을때 패링
        if(Input.GetKeyDown(KeyCode.Space)&& stateController.currentState == PlayerState.Entering)
        {
            
        }

    }
}
