using System.Collections;
using UnityEngine;

public class AttackTest : MonoBehaviour
{
    LineRenderer lr;
    [SerializeField]
    GameObject player;
    AttackData attackData = new AttackData(){attackDelay = 3f, aimDuration = 2f, attackWindupTime = 0.2f, parryStartTime = 0.1f, parryEndTime = 0.1f, hitTime = 0.5f};
    
    
    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.startColor = Color.red;
        lr.endColor = Color.red;
    }

    void Start()
    {
        StartCoroutine(AttackRoutine());
    }
    

    private IEnumerator AttackRoutine()
    {
        float elapsed = 0f;
        while (elapsed < attackData.aimDuration)
        {
            elapsed += Time.deltaTime;
            //조준선 그리기
            ShapeRenderHelper.DrawLine(lr, transform.position, player.transform.position);
            yield return null;
        }
        yield return new WaitForSeconds(attackData.attackWindupTime);

        //조준하다가 플레이어 공격
        AttackPlayer();
        //라인 렌더러 지우기
        lr.positionCount = 0;
        yield return new WaitForSeconds(attackData.attackDelay);
        StartCoroutine(AttackRoutine());
    }
    //조준 시간(조준 후, 발사 전 텀)
    //발사, 발사 후, 일정 시간후 패링 가능 시간 존재
    //패링 불가(이때 피격)

    void StartAttack()
    {
        EnemyAttackInfo attackInfo = new EnemyAttackInfo(){
            owner = this,
            startTime = Time.time,
            fireTime = Time.time + attackData.aimDuration + attackData.attackWindupTime,
            parryStartTime = Time.time + 0.3f,
            parryEndTime = Time.time + 0.4f,
            hitTime = Time.time + 1.0f
        };

        ParryManager.Instance.RegisterAttack(attackInfo);
    }

    private void AttackPlayer()
    {
        Debug.Log("플레이어 공격!");
    }
    
}

public class EnemyAttackInfo
{
    public AttackTest owner; // 공격을 한 적
    public float startTime;       // 공격 준비 시작
    public float fireTime;        // 발사 시점
    public float parryStartTime;  // 패링 가능 시간 시작
    public float parryEndTime;    // 패링 가능 시간 종료
    public float hitTime;         // 피격 발생 시점

    public bool IsParryable(float time)
    {
        return time >= parryStartTime && time <= parryEndTime;
    }
}

public class AttackData
{
    public float attackDelay;//공격이 반복되기까지의 시간
    public float aimDuration ;//조준하는 시간
    public float attackWindupTime; //조준 후 ~ 발사 전까지의 텀(Pre-Fire Delay)
    public float parryStartTime;
    public float parryEndTime;
    public float hitTime;
}
