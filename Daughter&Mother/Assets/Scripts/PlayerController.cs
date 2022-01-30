using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // �̵� �ӵ�
    public float moveSpeed;

    // ĳ���� ��Ʈ�ѷ� ����
    CharacterController cc;

    // �÷��̾� ü�� ����
    public int hp;

    // �ִ� ü�� ����
    int maxHp;

    // Hit ȿ�� ������Ʈ
     public GameObject hitEffect;

    // �ִϸ����� ����
    Animator anim;

    // �÷��̾� ������ Ȯ��
    bool playerMoving;

    // ������ ������ ���� Ȯ�� ����
    Vector2 lastMove;

    // �÷��̾� ���� Ȯ��
    bool playerAttacking;

    // �÷��̾� ���ݷ�
    public int attackPower = 3;

    // �÷��̾� ����
    public int defendPower;

    float currentAttackDelay;

    // ���� ������ �ð�
    public float attackDelay = 1f;

    public GameObject Player;
    public GameObject Enemy;

    // ��ȭâ
    public ChatManager chatManager;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Attack();
        ChangeObject();
    }

    void Move()
    {
        // ���� ����Ű ������ ������ �÷��̾�� �������� �������� ����
        playerMoving = false;
        playerAttacking = false;

        // ��ȭâ�� Ȱ��ȭ�� ���¶�� �÷��̾�� �������� �ʴ´�.
        if (chatManager.isAction)
        {
            playerMoving = false;
        }
        else //��ȭâ�� Ȱ��ȭ���� �ʾҴٸ� �÷��̾�� ������ �� �ִ�. 
        {
            // �¿�� �����̱�
            if (Input.GetAxisRaw("Horizontal") > 0f || Input.GetAxisRaw("Horizontal") < 0f)
            {
                // ����, �÷��̾��� hp�� 0 ���϶��...
                if (hp <= 0)
                {
                    // �÷��̾��� �ִϸ��̼��� �����.
                    anim.SetBool("isMove", false);
                    anim.SetBool("isAttack", false);
                }
                else
                {
                    transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));
                    playerMoving = true;
                    lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
                }
            }

            // ���Ϸ� �����̱�
            if (Input.GetAxisRaw("Vertical") > 0f || Input.GetAxisRaw("Vertical") < 0f)
            {
                // ����, �÷��̾��� hp�� 0 ���϶��...
                if (hp <= 0)
                {
                    // �÷��̾��� �ִϸ��̼��� �����.
                    anim.SetBool("isMove", false);
                    anim.SetBool("isAttack", false);
                }
                else
                {
                    transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f));
                    playerMoving = true;
                    lastMove = new Vector2(0f, Input.GetAxisRaw("Vertical"));
                }
            }
        }
        

        anim.SetFloat("DirX", Input.GetAxisRaw("Horizontal"));
        anim.SetFloat("DirY", Input.GetAxisRaw("Vertical"));
        anim.SetBool("isMove", playerMoving);
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);
    }

    void Attack()
    {
        playerMoving = false;
        playerAttacking = false;
        EnemyController ec = GameObject.Find("Enemy").GetComponent<EnemyController>();
        // Enemy2Controller ec = GameObject.Find("Enemy").GetComponent<Enemy2Controller>();
        // Enemy3Controller ec = GameObject.Find("Enemy").GetComponent<Enemy3Controller>();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            currentAttackDelay = attackDelay;
            // ���� �ִϸ��̼� Ȱ��ȭ
            playerAttacking = true;
            playerMoving = false;
            anim.SetFloat("DirX", Input.GetAxisRaw("Horizontal"));
            anim.SetFloat("DirY", Input.GetAxisRaw("Vertical"));
            anim.SetBool("isAttack", playerAttacking);
            if (Input.GetAxisRaw("Horizontal") > 0f || Input.GetAxisRaw("Horizontal") < 0f)
            {
                transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));
                playerMoving = true;
                lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
            }
            // �÷��̾�� ���ʹ��� �Ÿ��� 1���� ���� �� �÷��̾ �����ϸ�
            if (Vector2.Distance(Player.transform.position, Enemy.transform.position) <= 1)
            {
                // ���ʹ��� hp�� �÷��̾��� ���ݷ¸�ŭ �پ���.
                ec.hp -= attackPower;
            }
            // ��Ÿ�� ġ��(���ʹ��� hp�� 0�̸�) ��� ����� �ҷ��´�.
            if (ec.hp <= 0)
            {
                StartCoroutine(LastHitProcess());
            }
        }

        else
        {
            currentAttackDelay -= Time.deltaTime;
            if (currentAttackDelay <= 0)
            {
                anim.SetBool("isAttack", false);
                playerAttacking = false;
            }
        }
    }

    IEnumerator LastHitProcess()
    {
        EnemyController ec = GameObject.Find("Enemy").GetComponent<EnemyController>();
        // Enemy2Controller ec = GameObject.Find("Enemy").GetComponent<Enemy2Controller>();
        // Enemy3Controller ec = GameObject.Find("Enemy").GetComponent<Enemy3Controller>();

        yield return new WaitForSeconds(0.5f);

        // 1. ��� ��� UI�� Ȱ��ȭ�Ѵ�.
        ec.memory.SetActive(true);

        // 2. 5�ʰ� ����Ѵ�.
        yield return new WaitForSeconds(5f);

        // 3. ��� ��� UI�� ��Ȱ��ȭ�Ѵ�.
        ec.memory.SetActive(false);
    }

    // �÷��̾��� �ǰ� �Լ�
    public void DamageAction(int damage)
    {
        // ���ʹ��� ���ݷ¸�ŭ �÷��̾��� ü���� ��´�.
        hp -= damage - defendPower;

        // ����, �÷��̾��� ü���� 0���� ũ�� �ǰ� ȿ���� ����Ѵ�.
        if (hp > 0)
        {
            // �ǰ� ����Ʈ �ڷ�ƾ�� �����Ѵ�.
            StartCoroutine(PlayHitEffect());
        }
    }

    // �ǰ� ȿ�� �ڷ�ƾ �Լ�
    IEnumerator PlayHitEffect()
    {
        // 1. �ǰ� UI�� Ȱ��ȭ�Ѵ�.
        hitEffect.SetActive(true);

        // 2. 0.3�ʰ� ����Ѵ�.
        yield return new WaitForSeconds(0.3f);

        // 3. �ǰ� UI�� ��Ȱ��ȭ�Ѵ�.
        hitEffect.SetActive(false);
    }

    void ChangeObject()
    {
        // ���Ƿ� M�� ���� �� �������� ���ϵ��� ��.
        // ���� �κ��丮���� ������ ���� �� �������� ���ϴ� �ִϸ��̼����� ����ǵ��� ������ �����ؾ� ��.
        if(Input.GetKeyDown(KeyCode.M))
        {
            anim.SetBool("isChange", true);
            attackPower = 5;
        }
        else
        {
            return;
        }
    }
}
