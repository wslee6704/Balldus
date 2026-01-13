using UnityEngine;

public class EnemyAttackLoop : MonoBehaviour
{
    [SerializeField] private EnemyAttackPerformer performer;
    [SerializeField] private AttackDefinition attack;

    [SerializeField] private float interval = 5f;
    private float nextTime;

    void Awake()
    {
        if (!performer) performer = GetComponent<EnemyAttackPerformer>();
    }

    void Update()
    {
        if (!performer || !performer.IsAlive) return;

        if (Time.time >= nextTime)
        {
            Debug.Log($"현재시간, {Time.time}에 함수 등록됨");
            ParryManager.I.StartAttack(performer, attack);
            nextTime = Time.time + interval;
        }
    }
}