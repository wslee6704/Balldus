using System.Collections;
using UnityEngine;

public class AttackTest : MonoBehaviour
{

    [SerializeField]
    GameObject player;
    AttackData attackData = new AttackData() { attackDelay = 3f, aimDuration = 2f, attackWindupTime = 0.8f, parriableTime = 0.2f, hitTime = 0.5f };

    [Header("Raycast")]
    [SerializeField] private float maxDistance = 50f;
    [SerializeField] private LayerMask wallMask;

    void Awake()
    {

    }

    void Start()
    {

        StartCoroutine(AttackRoutine());
    }
    private Vector2 dir;//플레이어의 고정위치이므로, 조준 확정시간에는 변하지 않음
    void UpdateAimPosition(LineRenderer lr)
    {
        Vector2 origin = transform.position;
        dir = (player.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, maxDistance, wallMask);
        Vector3 endPos = hit.collider
            ? (Vector3)hit.point
            : (Vector3)(origin + dir * maxDistance);
        ShapeRenderHelper.DrawLine(lr, origin, endPos);
    }

    public bool CanHitPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, maxDistance);
        if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
        {
            return true;
        }
        return false;
    }




    private IEnumerator AttackRoutine()
    {
        LineRenderer lr;
        lr = LinePoolManager.I.Rent(transform);
        lr.startColor = Color.red;
        lr.endColor = Color.red;
        float elapsed = 0f;
        //매니저에 공격 등록
        RegisterAttack(attackData);
        while (elapsed < attackData.aimDuration)
        {
            elapsed += Time.deltaTime;
            //조준선 그리기
            UpdateAimPosition(lr);
            yield return null;
        }
        Debug.Log("쏜다!!");
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

    void RegisterAttack(AttackData attackData)
    {
        EnemyAttackInfo attackInfo = new EnemyAttackInfo()
        {
            owner = this,
            startTime = Time.time,
            fireTime = Time.time + attackData.aimDuration + attackData.attackWindupTime,
            parryStartTime = Time.time + attackData.aimDuration,
            parryEndTime = Time.time + attackData.aimDuration + attackData.attackWindupTime,
            hitTime = Time.time + attackData.aimDuration + attackData.attackWindupTime
        };

        ParryManager.Instance.RegisterAttack(attackInfo);
    }

    private void AttackPlayer()
    {
        if (CanHitPlayer())
        {
            Debug.Log("플레이어 공격성공!");
        }
        else
        {
            Debug.Log("회피!");
        }

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
    public float aimDuration;//조준하는 시간
    public float attackWindupTime; //조준 후 ~ 발사 전까지의 텀(Pre-Fire Delay)
    public float parriableTime; //패링 가능 시간
    public float hitTime;
}
