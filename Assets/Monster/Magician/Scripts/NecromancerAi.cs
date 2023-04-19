using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerAi : BattleSystem
{
    Rigidbody2D rigid;

    public int nextMove;
    private float nextFireTime; //������ 

    public GameObject player;
    public NecroBulletFire necroFire;

    public bool isAttack = false;
    

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        Invoke("checkFloorCollider", 2);
    }

    // Update is called once per frame
    void Update()
    {

        if (isAttack && OnLive())
        {
            if (Time.time > nextFireTime)
            {
                nextFireTime = Time.time + necroFire.fireRate;
                //Debug.Log(nextFireTime);
                Attack();

            }
            
        }


    }

    private void FixedUpdate() //���� �̵� ����
    {
        if (!isAttack &&OnLive())
            Move();
        else
            GetComponent<Animator>().SetBool("Move", false);
    }

    void checkFloorCollider() //�ٴ� �ݶ��̴� Ȯ��(�ȶ�������)
    {
        nextMove = Random.Range(-2, 2);
        Invoke("checkFloorCollider", 2);
    }

    void Move()
    {
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
        GetComponent<SpriteRenderer>().flipX = nextMove < 0;

        Vector2 frontVec = new Vector2(rigid.position.x + nextMove, rigid.position.y);


        Debug.DrawRay(frontVec, Vector3.down * 5f, Color.green);
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 5, LayerMask.GetMask("Default"));
        if (rayHit.collider == null)
        {
            Debug.Log("���� ���������� ����!");
            nextMove *= -1;
            CancelInvoke();
            Invoke("checkFloorCollider", 2);
        }
    }

    void Attack()
    {
        GetComponent<Animator>().SetTrigger("Attack1"); //Ʈ���� �ߵ�
        nextMove = 0;
        //Debug.Log(transform.position.x - player.transform.position.x);
        GetComponent<SpriteRenderer>().flipX = transform.position.x - player.transform.position.x > 0;
        necroFire.Fire();
        CancelInvoke();
        Invoke("checkFloorCollider", 2);
    }

}
