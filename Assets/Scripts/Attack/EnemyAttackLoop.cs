using UnityEngine;

public class EnemyAttackLoop : MonoBehaviour
{
    [SerializeField] private EnemyAttackPerformer performer;
    [SerializeField] private AttackDefinition attack;

    [SerializeField] private float interval = 3f;
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
            CombatDirector.I.StartAttack(performer, attack);
            nextTime = Time.time + interval;
        }
    }
}