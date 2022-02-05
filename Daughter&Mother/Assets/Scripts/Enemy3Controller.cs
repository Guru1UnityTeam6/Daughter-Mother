using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy3Controller : MonoBehaviour
{
    // 에너미 상태 변수
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Damaged,
        Die
    }

    // 에너미 상태 변수
    EnemyState m_State;

    // 플레이어 발견 범위
    public float findDistance = 8f;

    // 플레이어 트랜스폼
    Transform player;

    // 공격 가능 범위
    public float attackDistance = 1f;

    // 이동 속도
    public float moveSpeed = 5f;

    // 캐릭터 콘트롤러 컴포넌트
    CharacterController cc;

    // 누적 시간
    float currentTime = 0;

    // 공격 딜레이 시간
    float attackDelay = 2f;

    // 에너미 공격력
    public int attackPower = 3;

    // 에너미의 체력
    public int hp = 15;

    // 에너미의 최대 체력
    //int maxHp = 15;

    // 애니메이터 변수
    Animator anim;

    // 에너미 움직임 확인
    bool enemyMoving;

    // 마지막 움직임 방향 확인 변수
    Vector2 lastMove;

    // 가속 누적 시간
    float accCurrentTime = 0;

    // 가속 딜레이 시간
    float accDelay = 5f;

    void Start()
    {
        // 최초의 에너미 상태는 대기로 한다.
        m_State = EnemyState.Idle;

        // 플레이어의 트랜스폼 컴포넌트 받아오기
        player = GameObject.Find("Player").transform;

        // 캐릭터 콘트롤러 컴포넌트 받아오기
        cc = GetComponent<CharacterController>();

        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 현재 상태를 체크해 상태별로 정해진 기능을 수행하게 하고 싶다.
        switch (m_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Damaged:
                //Damaged();
                break;
            case EnemyState.Die:
                //Die();
                break;
        }
    }

    void Idle()
    {
        // 만일, 플레이어와의 거리가 액션 시작 범위 이내라면 Move 상태로 전환한다.
        if (Vector3.Distance(transform.position, player.position) < findDistance)
        {
            m_State = EnemyState.Move;
            print("상태 전환: Idle -> Move");
            enemyMoving = true;
        }
        else
        {
            enemyMoving = false;
        }
    }

    void Acceleration()
    {
        accCurrentTime += Time.deltaTime;
        if (accCurrentTime > accDelay)
        {
            moveSpeed = 5f;
            Debug.Log("순간가속!");
            if (accCurrentTime - accDelay > 2f) // 5초 이후 2초간 빨라짐
            {
                accCurrentTime = 0;
            }
        }
        else
        {
            moveSpeed = 1f;
            Debug.Log("순간가속멈춤" + string.Format("{0:N1}", moveSpeed) + string.Format("{0:N1}", accCurrentTime));
        }
    }
    void Move()
    {
        enemyMoving = true;

        Vector3 dir = Vector3.zero;

        // 만일, 플레이어와의 거리가 공격 범위 밖이라면 플레이어를 향해 이동한다.
        if (Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            // 이동 방향 설정
            dir = (player.position - transform.position).normalized;

            //순간가속 몇초에 한번씩 할 수 있도록 해야함
            Acceleration();

            // 캐릭터 콘트롤러를 이용해 이동하기
            cc.Move(dir * moveSpeed * Time.deltaTime);
            lastMove = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        }
        // 그렇지 않다면, 현재 상태를 공격(Attack)으로 전환한다.
        else
        {
            m_State = EnemyState.Attack;
            print("상태 전환: Move -> Attack");

            // 누적 시간을 공격 딜레이 시간만큼 미리 진행시켜 놓는다.
            currentTime = attackDelay;
        }
        anim.SetFloat("DirX", dir.x);
        anim.SetFloat("DirY", dir.y);
        anim.SetBool("isMove", enemyMoving);
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);
    }

    void Attack()
    {
        enemyMoving = true;

        // 만일, 플레이어가 공격 범위 이내에 있다면 플레이어를 공격한다.
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            // 일정 시간마다 플레이어를 공격한다.
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                player.GetComponent<PlayerController>().DamageAction(attackPower);
                //공격당하면서 체력바 줄어듬
                PlayerStat.instance.currentHP -= attackPower;
                print(PlayerStat.instance.currentHP + "이 남았습니다");
                print("공격");
                currentTime = 0;
            }
        }
        // 그렇지 않다면, 현재 상태를 이동(Move)으로 전환한다(재추격 실시).
        else
        {
            m_State = EnemyState.Move;
            print("상태 전환: Attack -> Move");
            currentTime = 0;
        }
    }

    void Damaged()
    {
        // 피격 상태를 처리하기 위한 코루틴을 실행한다.
        StartCoroutine(DamageProcess());
    }

    // 데미지 처리용 코루틴 함수
    IEnumerator DamageProcess()
    {
        yield return new WaitForSeconds(0f);

        // 현재 상태를 이동 상태로 전환한다.
        m_State = EnemyState.Move;
        print("상태 전환: Damaged -> Move");
    }

    // 데미지 실행 함수
    public void HitEnemy(int hitPower)
    {
        // 만일, 이미 피격 상태이거나 사망 상태 또는 복귀 상태라면 아무런 처리도 하지 않고 함수를 종료한다.
        if (m_State == EnemyState.Damaged || m_State == EnemyState.Die)
        {
            return;
        }

        // 플레이어의 공격력만큼 에너미의 체력을 감소시킨다.
        hp -= hitPower;
        //PlayerStat.instance.currentHP -= hitPower;

        // 에너미의 체력이 0보다 크면 피격 상태로 전환한다.
        if (hp > 0)
        {
            m_State = EnemyState.Damaged;
            print("상태 전환: Any state -> Damaged");

            Damaged();
        }
        // 그렇지 않다면 죽음 상태로 전환한다.
        else
        {
            m_State = EnemyState.Die;
            print("상태 전환: Any state -> Die");

            Die();
        }
    }

    // 죽음 상태 함수
    void Die()
    {
        // 진행 중인 피격 코루틴을 중지한다.
        StopAllCoroutines();

        // 죽음 상태를 처리하기 위한 코루틴을 실행한다.
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        print("소멸!");

        // 캐릭터 콘트롤러 컴포넌트를 비활성화시킨다.
        cc.enabled = false;

        // 2초 동안 기다린 후에 자기 자신을 제거한다.
        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }
}
