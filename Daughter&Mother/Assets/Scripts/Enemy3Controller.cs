using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy3Controller : MonoBehaviour
{
    // ���ʹ� ���� ����
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Damaged,
        Die
    }

    // ���ʹ� ���� ����
    EnemyState m_State;

    // �÷��̾� �߰� ����
    public float findDistance = 8f;

    // �÷��̾� Ʈ������
    Transform player;

    // ���� ���� ����
    public float attackDistance = 1f;

    // �̵� �ӵ�
    public float moveSpeed = 5f;

    // ĳ���� ��Ʈ�ѷ� ������Ʈ
    CharacterController cc;

    // ���� �ð�
    float currentTime = 0;

    // ���� ������ �ð�
    float attackDelay = 2f;

    // ���ʹ� ���ݷ�
    public int attackPower = 3;

    // ���ʹ��� ü��
    public int hp = 15;

    // ���ʹ��� �ִ� ü��
    //int maxHp = 15;

    // �ִϸ����� ����
    Animator anim;

    // ���ʹ� ������ Ȯ��
    public bool enemyMoving;

    // ������ ������ ���� Ȯ�� ����
    Vector2 lastMove;

    // ���� ���� �ð�
    float accCurrentTime = 0;

    //ü�¹�
    public Slider EnemyHpSlider;
    public Text EnemyHpText;

    // ���� ������ �ð�
    float accDelay = 5f;

    // ������ �̹��� UI
    public GameObject memory;

    // Item, Stuff -> �÷��̾ �����̸� ��Ȱ��ȭ
    GameObject Item;
    GameObject Stuff;

    void Start()
    {
        // ������ ���ʹ� ���´� ���� �Ѵ�.
        m_State = EnemyState.Idle;

        // �÷��̾��� Ʈ������ ������Ʈ �޾ƿ���
        player = GameObject.Find("Player").transform;

        // ĳ���� ��Ʈ�ѷ� ������Ʈ �޾ƿ���
        cc = GetComponent<CharacterController>();

        anim = GetComponent<Animator>();

        Item = GameObject.Find("Item");
        Stuff = GameObject.Find("Stuff");

        // �÷��̾��� ���ʹ̸� �� ��ü�� ����
        if (findDistance != 0)
        {
            PlayerController.instance.Enemy = gameObject; 
        }

        // playercontroller�� ec3 ����
        PlayerController.instance.ec3 = GameObject.Find("Enemy3").GetComponent<Enemy3Controller>();

        // ���ʹ� ü�¹� �����̴��� ��Ȱ��ȭ ���·�
        EnemyHpSlider.gameObject.SetActive(false);
    }

    void Update()
    {
        // ���� ���¸� üũ�� ���º��� ������ ����� �����ϰ� �ϰ� �ʹ�.
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

        // EnemySlider UI ����
        EnemyHpSlider.maxValue = 15; // Enemy1�� �� HP�� 15
        EnemyHpSlider.value = hp;
        string a = hp.ToString();
        EnemyHpText.text = a;

        // �÷��̾�� ���ʹ��� HPSlider ����
        if (enemyMoving == true) // ���ʹ̰� �����̴� ���� : ������
        {
            // �÷��̾��� hpSlider ����
            PlayerStat.instance.hpSlider.gameObject.SetActive(true);
            // ���ʹ��� hpSlider ����
            EnemyHpSlider.gameObject.SetActive(true);
        }
        // ���ʹ��� hp�� 0�̸� EnemyHpSlider ��Ȱ��ȭ
        if (hp < 0)
        {
            EnemyHpSlider.gameObject.SetActive(false);
        }

        // ���ʹ̰� �����̴� ���¶�� Item, Stuff ������Ʈ ��Ȱ��ȭ
        if (enemyMoving == false) 
        {
            Item.SetActive(true);
            Stuff.SetActive(true);
        }
        else
        {
            Item.SetActive(false);
            Stuff.SetActive(false);
        }
    }

    void Idle()
    {
        // ����, �÷��̾���� �Ÿ��� �׼� ���� ���� �̳���� Move ���·� ��ȯ�Ѵ�.
        if (Vector3.Distance(transform.position, player.position) < findDistance)
        {
            m_State = EnemyState.Move;
            print("���� ��ȯ: Idle -> Move");
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
            Debug.Log("��������!");
            if (accCurrentTime - accDelay > 2f) // 5�� ���� 2�ʰ� ������
            {
                accCurrentTime = 0;
            }
        }
        else
        {
            moveSpeed = 1f;
            Debug.Log("�������Ӹ���" + string.Format("{0:N1}", moveSpeed) + string.Format("{0:N1}", accCurrentTime));
        }
    }
    
    void Move()
    {
        if (Inventory.instance.activeInventory)
        {
            enemyMoving = false;
        }
        else
        {
            enemyMoving = true;

            Vector3 dir = Vector3.zero;

            // ����, �÷��̾���� �Ÿ��� ���� ���� ���̶�� �÷��̾ ���� �̵��Ѵ�.
            if (Vector3.Distance(transform.position, player.position) > attackDistance)
            {
                // �̵� ���� ����
                dir = (player.position - transform.position).normalized;

                //�������� ���ʿ� �ѹ��� �� �� �ֵ��� �ؾ���
                Acceleration();

                // ĳ���� ��Ʈ�ѷ��� �̿��� �̵��ϱ�
                cc.Move(dir * moveSpeed * Time.deltaTime);
                lastMove = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
            }
            // �׷��� �ʴٸ�, ���� ���¸� ����(Attack)���� ��ȯ�Ѵ�.
            else
            {
                m_State = EnemyState.Attack;
                print("���� ��ȯ: Move -> Attack");

                // ���� �ð��� ���� ������ �ð���ŭ �̸� ������� ���´�.
                currentTime = attackDelay;
            }
            anim.SetFloat("DirX", dir.x);
            anim.SetFloat("DirY", dir.y);
            anim.SetBool("isMove", enemyMoving);
            anim.SetFloat("LastMoveX", lastMove.x);
            anim.SetFloat("LastMoveY", lastMove.y);
        }
    }

    void Attack()
    {
        if (Inventory.instance.activeInventory)
        {
            enemyMoving = false;
            Debug.Log(enemyMoving);
        }
        else
        {
            enemyMoving = true;

            // ����, �÷��̾ ���� ���� �̳��� �ִٸ� �÷��̾ �����Ѵ�.
            if (Vector3.Distance(transform.position, player.position) < attackDistance)
            {
                // ���� �ð����� �÷��̾ �����Ѵ�.
                currentTime += Time.deltaTime;
                if (currentTime > attackDelay)
                {
                    // �÷��̾� �ǰ� -> HP ����
                    player.GetComponent<PlayerController>().DamageAction(attackPower);
                    print(PlayerStat.instance.currentHP + "�� ���ҽ��ϴ�");
                    print("����");
                    currentTime = 0;
                }
            }
            // �׷��� �ʴٸ�, ���� ���¸� �̵�(Move)���� ��ȯ�Ѵ�(���߰� �ǽ�).
            else
            {
                m_State = EnemyState.Move;
                print("���� ��ȯ: Attack -> Move");
                currentTime = 0;
            }
        }
    }

    void Damaged()
    {
        // �ǰ� ���¸� ó���ϱ� ���� �ڷ�ƾ�� �����Ѵ�.
        StartCoroutine(DamageProcess());
    }

    // ������ ó���� �ڷ�ƾ �Լ�
    IEnumerator DamageProcess()
    {
        yield return new WaitForSeconds(0f);

        // ���� ���¸� �̵� ���·� ��ȯ�Ѵ�.
        m_State = EnemyState.Move;
        print("���� ��ȯ: Damaged -> Move");
    }

    // ������ ���� �Լ�
    public void HitEnemy(int hitPower)
    {
        // ����, �̹� �ǰ� �����̰ų� ��� ���� �Ǵ� ���� ���¶�� �ƹ��� ó���� ���� �ʰ� �Լ��� �����Ѵ�.
        if (m_State == EnemyState.Damaged || m_State == EnemyState.Die)
        {
            return;
        }

        // �÷��̾��� ���ݷ¸�ŭ ���ʹ��� ü���� ���ҽ�Ų��.
        hp -= hitPower;

        // ���ʹ��� ü���� 0���� ũ�� �ǰ� ���·� ��ȯ�Ѵ�.
        if (hp > 0)
        {
            m_State = EnemyState.Damaged;
            print("���� ��ȯ: Any state -> Damaged");

            Damaged();
        }
        // �׷��� �ʴٸ� ���� ���·� ��ȯ�Ѵ�.
        else
        {
            m_State = EnemyState.Die;
            print("���� ��ȯ: Any state -> Die");

            Die();
        }
    }

    // ���� ���� �Լ�
    void Die()
    {
        // ���� ���� �ǰ� �ڷ�ƾ�� �����Ѵ�.
        StopAllCoroutines();

        // ���� ���¸� ó���ϱ� ���� �ڷ�ƾ�� �����Ѵ�.
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        print("�Ҹ�!");

        // ĳ���� ��Ʈ�ѷ� ������Ʈ�� ��Ȱ��ȭ��Ų��.
        cc.enabled = false;

        // 2�� ���� ��ٸ� �Ŀ� �ڱ� �ڽ��� �����Ѵ�.
        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }
}
