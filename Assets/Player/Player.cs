using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : BattleSystem 
{

    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;
    [SerializeField] float      m_rollForce = 6.0f;    
    [SerializeField] GameObject m_slideDust;

    /*                sound                 */
    AudioSource audioSource;

    public float walkSoundDelay = 0.05f;
    private float m_lastWalkSoundTime = -1f;
    public AudioSource walkSound;

    public AudioSource attackSound;
    public AudioSource defenceSound;
    public AudioSource hurtSound;
    public AudioSource jumpSound;
    public AudioSource landedSound;
    /*                                      */

    /*             Player Component         */
    BoxCollider2D playerCollider;
    Rigidbody2D playerRig2d;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_HeroKnight   m_groundSensor;
    private Sensor_HeroKnight   m_wallSensorR1;
    private Sensor_HeroKnight   m_wallSensorR2;
    private Sensor_HeroKnight   m_wallSensorL1;
    private Sensor_HeroKnight   m_wallSensorL2;
    private bool                m_isWallSliding = false;
    private bool                m_grounded = false;
    private bool                m_rolling = false;
    private int                 m_facingDirection = 1;
    private int                 m_currentAttack = 0;
    private float               m_timeSinceAttack = 0.0f;
    private float               m_delayToIdle = 0.0f;
    private float               m_rollDuration = 8.0f / 14.0f;
    private float               m_rollCurrentTime;
    private float invincibleTime = 0.5f;
    private bool  isInvincible = false;
    public LayerMask enemyLayer;
    public float attackRange;
    public GameObject attackPoint;


    public string sceneName;


    public enum STATE
    {
        Idle,Death,
    }
    public STATE myState = STATE.Idle;
    void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;
        switch(s)
        {
            case STATE.Idle:
                break;
            case STATE.Death:
                GetComponent<Animator>().SetTrigger("Death");
                GameManager.instance.gameOverScreen.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// 키 입력에 따른 플레이어 스테이트 변화 함수
    /// </summary>
    void StateProcess()
    {
        switch (myState)
        {
            case STATE.Idle:
                // Increase timer that controls attack combo
                m_timeSinceAttack += Time.deltaTime;
                // Increase timer that checks roll duration
                if (m_rolling)
                    m_rollCurrentTime += Time.deltaTime;

                // Disable rolling if timer extends duration
                if (m_rollCurrentTime > m_rollDuration)
                    m_rolling = false;

                //Check if character just landed on the ground
                if (!m_grounded && m_groundSensor.State())
                {
                    landedSound.Play();
                    m_grounded = true;
                    m_animator.SetBool("Grounded", m_grounded);
                }

                //Check if character just started falling
                if (m_grounded && !m_groundSensor.State())
                {
                    m_grounded = false;
                    m_animator.SetBool("Grounded", m_grounded);
                }

                // -- Handle input and movement --
                float inputX = Input.GetAxis("Horizontal");

                // Swap direction of sprite depending on walk direction
                if (inputX > 0)
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                    m_facingDirection = 1;
                }

                else if (inputX < 0)
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                    m_facingDirection = -1;
                }

                // Move
                if (!m_rolling)
                    m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

                //Set AirSpeed in animator
                m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

                // -- Handle Animations --
                //Wall Slide
                m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
                m_animator.SetBool("WallSlide", m_isWallSliding);

                if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling)
                {

                    attackSound.Play();
                    m_currentAttack++;
                    Attack();
                    // Loop back to one after third attack
                    if (m_currentAttack > 3)
                        m_currentAttack = 1;

                    // Reset Attack combo if time since last attack is too large
                    if (m_timeSinceAttack > 1.0f)
                        m_currentAttack = 1;

                    // Call one of three attack animations "Attack1", "Attack2", "Attack3"
                    m_animator.SetTrigger("Attack" + m_currentAttack);

                    // Reset timer
                    m_timeSinceAttack = 0.0f;
                }

                // Block
                else if (Input.GetMouseButtonDown(1) && !m_rolling)
                {
                    defenceSound.Play();
                    StartCoroutine(Invincibility());
                    m_animator.SetTrigger("Block");
                    m_animator.SetBool("IdleBlock", true);
                }

                else if (Input.GetMouseButtonUp(1))
                    m_animator.SetBool("IdleBlock", false);

                // Roll
                else if (Input.GetKeyDown("left shift") && !m_rolling && !m_isWallSliding)
                {
                    m_rolling = true;
                    m_animator.SetTrigger("Roll");
                    m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
                }


                //Jump
                else if (Input.GetKeyDown("space") && m_grounded && !m_rolling)
                {
                    jumpSound.Play();
                    m_animator.SetTrigger("Jump");
                    m_grounded = false;
                    m_animator.SetBool("Grounded", m_grounded);
                    m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
                    m_groundSensor.Disable(0.2f);
                }

                //Run
                else if (Mathf.Abs(inputX) > Mathf.Epsilon)
                {
                    if ((m_lastWalkSoundTime < 0f || Time.time - m_lastWalkSoundTime >= walkSoundDelay) &&
                        m_grounded)
                    {
                        walkSound.Play();
                        m_lastWalkSoundTime = Time.time;
                    }
                    // Reset timer
                    m_delayToIdle = 0.05f;
                    m_animator.SetInteger("AnimState", 1);
                }

                //Idle
                else
                {
                    // Prevents flickering transitions to idle
                    m_delayToIdle -= Time.deltaTime;
                    if (m_delayToIdle < 0)
                        m_animator.SetInteger("AnimState", 0);
                }
                break;
            case STATE.Death:

                if(Input.GetKey(KeyCode.R))
                {
                    GameManager.instance.StartGame();
                    SceneManager.LoadScene(sceneName);
                }
                break;


        }
    }

    // Use this for initialization
    void Start ()
    {
        HP = GameManager.instance.GetLife();
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();

        audioSource = GetComponent<AudioSource>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerRig2d = GetComponent<Rigidbody2D>();

        sceneName = GameManager.instance.nowSceneName;
    }

    // Update is called once per frame
    void Update ()
    {
        StateProcess();        
    }

    /// <summary>
    /// 플레이어가 데미지를 입을시 발동하는 함수.
    /// BattleSystem에서 상속을 받아서 재정의 후 사용
    /// </summary>
    public override void OnDamage()
    {
        hurtSound.Play();
        if (!OnLive()) return;
        if (isInvincible) return;
        StartCoroutine(Invincibility());
        HP--;
        GameManager.instance.RemoveLife();
        if (OnLive())
        {
            GetComponent<Animator>().SetTrigger("Hurt");
        }
        else
        {
            playerCollider.enabled = false;
            playerRig2d.gravityScale = 0;
            ChangeState(STATE.Death);   
        }
    }

    /// <summary>
    /// 플레이어 공격시 몬스터에 데미지 입히는 과정
    /// </summary>
    private void Attack()
    {
        Vector2 pos = attackPoint.transform.position;
        pos.x = GetComponent<SpriteRenderer>().flipX ? transform.position.x - attackPoint.transform.localPosition.x : transform.position.x + attackPoint.transform.localPosition.x;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(pos, attackRange, enemyLayer); // 공격 범위 내의 적을 모두 탐지

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<BattleSystem>()?.OnDamage(); // 탐지된 적에게 데미지 적용
        }
    }

    /// <summary>
    /// // 공격 범위를 시각화
    /// 플레이어 앞부분에 생성되는
    /// Red Circle의 정체
    /// </summary>
    private void OnDrawGizmosSelected() 
    {
        Vector2 pos = attackPoint.transform.position;
        pos.x = GetComponent<SpriteRenderer>().flipX ? transform.position.x-attackPoint.transform.localPosition.x : transform.position.x+ attackPoint.transform.localPosition.x;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pos, attackRange);
    }

    /// <summary>
    /// 무적상태로 잠시 만들어주는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator Invincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible=false;
    }


    // Animation Events
    // Called in slide animation.
    /// <summary>
    /// 플레이어가 벽(collider)에 붙어있을시
    /// 슬라이드 이펙트와 애니메이션을 생성
    /// </summary>
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }

    /// <summary>
    /// 단발성 사운드를 플레이하기 위한 함수 (공격, 아이템 획득, 순간이동 등)
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip); //한번만 실행되게 될것
    }
}
