using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove : BattleSystem
{
    public float speed = 1.0f;
    private float nextFireTime; //������ 

    public AudioSource bossWalk;
    public float walkSoundDelay = 0.01f;
    private float m_lastWalkSoundTime = -1f;

    public bool isAttack = false;

    public GameObject player;

    public BossBulletFire BossBulletFireScript;

    private Vector3 dir;


    // Start is called before the first frame update

    // Update is called once per frame
    void Update() //���� ���� ��ȭ ����
    {
        //�÷��̾� �������� ��� ��ġ ����
        dir = transform.position - player.transform.position;


        //�÷��̾� ��ġ�� ��������Ʈ�� �ٶ󺸰�
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

    private void FixedUpdate() //���� �̵� ����
    {
        if(!isAttack && OnLive())
            Move();
        else
            GetComponent<Animator>().SetBool("Move", false);
    }


    void Attack()
    {
        transform.position = transform.position; //���ڸ�
        GetComponent<Animator>().SetTrigger("Attack"); //Ʈ���� �ߵ�
        if (Time.time > nextFireTime)
        {
            nextFireTime = Time.time + BossBulletFireScript.fireRate;
            Debug.Log(nextFireTime);
            BossBulletFireScript.Fire();
        }
    }

    /// <summary>
    /// ���� ���� �̵�
    /// </summary>
    void Move()
    {
        if (m_lastWalkSoundTime < 0f || Time.time - m_lastWalkSoundTime >= walkSoundDelay)
        {
            Debug.Log("�ȴ� �Ҹ� ���");
            bossWalk.Play();
            m_lastWalkSoundTime = Time.time;
        }

        GetComponent<Animator>().SetBool("Move", true);
        transform.Translate(dir.normalized * Time.deltaTime * speed);
    }

}
