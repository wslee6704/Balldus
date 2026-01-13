using UnityEngine;

public class RangeBullet : MonoBehaviour
{

    protected Rigidbody2D rigid;
    BulletFireExecute bullet;
    protected float bulletSpeed = 0;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

    }

    //데미지수치, 관통수치 초기화 함수
    public void Init(Vector3 dir, float bulletSpeed, BulletFireExecute bullet)
    {
        this.bulletSpeed = bulletSpeed;
        this.bullet = bullet;
        //총알 회전
        this.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        //총알 발사
        rigid.linearVelocity = dir * bulletSpeed;
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //총알 -> 모듈 -> 인스턴스
            bullet.CallInstanceEvent();
            Debug.Log("플레이어와 총알 충돌!");
            DisposeBullet();
            return;
        }

        if(collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            Debug.Log("벽과 총알 충돌!");
            DisposeBullet();
        }
        //inst에 타격 이벤트 호출
    }

    public void DisposeBullet()
    {
        Destroy(this.gameObject);
    }
}
