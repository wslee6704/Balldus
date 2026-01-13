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
        if (Input.GetKeyDown(KeyCode.Space) && stateController.currentState == PlayerState.Entering)
        {

            if (ParryManager.I.TryParry())
            {
                AudioManager.instance.PlaySfx(AudioManager.Sfx.ParrySuccess);
                Debug.Log("Parry Success!");
            }
            else
            {
                Debug.Log("Parry할 공격 없음.");
            }
        }

    }
}
