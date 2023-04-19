using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove : BattleSystem
{
    public float speed = 1.0f;
    private float nextFireTime; //딜레이 

    public AudioSource bossWalk;
    public float walkSoundDelay = 0.01f;
    private float m_lastWalkSoundTime = -1f;

    public bool isAttack = false;

    public GameObject player;

    public BossBulletFire BossBulletFireScript;

    private Vector3 dir;


    // Start is called before the first frame update

    // Update is called once per frame
    void Update() //보스 상태 변화 구현
    {
        //플레이어 방향으로 계속 위치 지정
        dir = transform.position - player.transform.position;


        //플레이어 위치로 스프라이트가 바라보게
        if(OnLive())
            GetComponent<SpriteRenderer>().flipX = transform.position.x - player.transform.position.x < 0;



        if(isAttack && OnLive())
        {
            Attack();
        }
        else
        {
            GetComponent<Animator>().ResetTrigger("Attack");
        }


    }

    private void LateUpdate()
    {
        
    }

    private void FixedUpdate() //보스 이동 구현
    {
        if(!isAttack && OnLive())
            Move();
        else
            GetComponent<Animator>().SetBool("Move", false);
    }


    void Attack()
    {
        transform.position = transform.position; //제자리
        GetComponent<Animator>().SetTrigger("Attack"); //트리거 발동
        if (Time.time > nextFireTime)
        {
            nextFireTime = Time.time + BossBulletFireScript.fireRate;
            Debug.Log(nextFireTime);
            BossBulletFireScript.Fire();
        }
    }

    /// <summary>
    /// 보스 몬스터 이동
    /// </summary>
    void Move()
    {
        if (m_lastWalkSoundTime < 0f || Time.time - m_lastWalkSoundTime >= walkSoundDelay)
        {
            Debug.Log("걷는 소리 출력");
            bossWalk.Play();
            m_lastWalkSoundTime = Time.time;
        }

        GetComponent<Animator>().SetBool("Move", true);
        transform.Translate(dir.normalized * Time.deltaTime * speed);
    }

}
