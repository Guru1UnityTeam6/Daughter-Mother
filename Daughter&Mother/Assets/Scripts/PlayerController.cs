using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // �ν��Ͻ�
    public static PlayerController instance;

    // �̵� �ӵ�
    public float moveSpeed;
    // ĳ���� ��Ʈ�ѷ� ����
    CharacterController cc;
    // Hit ȿ�� ������Ʈ
     public GameObject hitEffect;
    // �ִϸ����� ����
    protected Animator anim;
    // �÷��̾� ������ Ȯ��
    protected bool playerMoving;
    // ������ ������ ���� Ȯ�� ����
    protected Vector2 lastMove;
    // �÷��̾� ���� Ȯ��
    protected bool playerAttacking;
    // ���� ������ �ð�
    protected float currentAttackDelay;
    public float attackDelay = 1f;

    public GameObject Player; 
    public GameObject Enemy;

    public Enemy1Controller ec1;
    public Enemy2Controller ec2;
    public Enemy3Controller ec3;

    // ��ȭâ
    public ChatManager chatManager;
    // ������ ��
    public bool aftermemory;
    // �ֱ� ����� Door�� �̸� ���� -> DoorPoint���� ���
    public string currentDoor;

    void Start()
    {
        anim = GetComponent<Animator>();
        // ���� �ٲ� ������ chatManager�� ��������� ���ݾ�... -> chatManager�� Start����
        // ���࿡ dontDestroy �� �Ѵٸ� �������� �ʾƵ� �ǰ���... but �ɸ��°� �ʹ� ���� �ʾ�?
        instance = this; 
        aftermemory = false; 
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

        // ��ȭâ, �κ��丮�� Ȱ��ȭ�� ���¶�� �÷��̾�� �������� �ʴ´�.
        if ((chatManager.isAction) || (Inventory.instance.activeInventory)
            || (chatManager.talkPanel.activeSelf) || (chatManager.playerPanel1.activeSelf) || (chatManager.playerPanel2.activeSelf)
            || (chatManager.playerPanel3.activeSelf) || (chatManager.motherPanel1.activeSelf) || (chatManager.motherPanel3.activeSelf)
            || (chatManager.motherPanel3.activeSelf))
        {
            playerMoving = false;
        }
        else //��ȭâ�� Ȱ��ȭ���� �ʾҴٸ� �÷��̾�� ������ �� �ִ�. 
        {
            // �¿�� �����̱�
            if (Input.GetAxisRaw("Horizontal") > 0f || Input.GetAxisRaw("Horizontal") < 0f)
            {
                // ����, �÷��̾��� hp�� 0 ���϶��...
                if (PlayerStat.instance.currentHP <= 0)
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
                if (PlayerStat.instance.currentHP <= 0)
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
        //���� Enemy1, 2, 3�� ���� z�� ���� ������ �� �ִ�.
        if ((SceneManager.GetActiveScene().name == "Enemy1") || (SceneManager.GetActiveScene().name == "Enemy2") || (SceneManager.GetActiveScene().name == "Enemy3"))
        {
            playerMoving = false;
            playerAttacking = false;
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("���ʹ� : " + Enemy);
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
                    Debug.Log("�÷��̾�� ���ʹ� �Ÿ��� 1");
                    // ���ʹ��� hp�� �÷��̾��� ���ݷ¸�ŭ �پ���.
                    if (SceneManager.GetActiveScene().name == "Enemy1")
                    {
                        ec1.hp -= PlayerStat.instance.AKT;
                    }
                    else if (SceneManager.GetActiveScene().name == "Enemy2")
                    {
                        Debug.Log("������");
                        ec2.hp -= PlayerStat.instance.AKT;
                    }
                    else if (SceneManager.GetActiveScene().name == "Enemy3")
                    {
                        ec3.hp -= PlayerStat.instance.AKT;
                    }
                }
                // ��Ÿ�� ġ��(���ʹ��� hp�� 0�̸�) ��� ����� �ҷ��´�.
                if (ec1.hp <= 0)
                {
                    ec1.enemyMoving = false;
                    ec1.Idle();
                    StartCoroutine(LastHitProcess());
                }
                else if (ec2.hp <= 0)
                {
                    ec2.enemyMoving = false;
                    ec2.Idle();
                    StartCoroutine(LastHitProcess());
                }
                else if (ec3.hp <= 0)
                {
                    ec3.enemyMoving = false;
                    ec3.Idle();
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
    }


    IEnumerator LastHitProcess()
    {
        yield return new WaitForSeconds(0.5f);
        if (SceneManager.GetActiveScene().name == "Enemy1")
        {
            ec1.enemyMoving = false;
            // 1. ��� ��� UI�� Ȱ��ȭ�Ѵ�.
            ec1.memory.SetActive(true);
            // 2. 5�ʰ� ����Ѵ�.
            yield return new WaitForSeconds(3f);
            // 3. ��� ��� UI�� ��Ȱ��ȭ�Ѵ�.
            ec1.memory.SetActive(false);
        }
        else if (SceneManager.GetActiveScene().name == "Enemy2")
        {
            ec2.enemyMoving = false;
            // 1. ��� ��� UI�� Ȱ��ȭ�Ѵ�.
            ec2.memory.SetActive(true);
            // 2. 5�ʰ� ����Ѵ�.
            yield return new WaitForSeconds(3f);
            // 3. ��� ��� UI�� ��Ȱ��ȭ�Ѵ�.
            ec2.memory.SetActive(false);
        }
        else if (SceneManager.GetActiveScene().name == "Enemy3")
        {
            ec3.enemyMoving = false;
            // 1. ��� ��� UI�� Ȱ��ȭ�Ѵ�.
            ec3.memory.SetActive(true);
            // 2. 5�ʰ� ����Ѵ�.
            yield return new WaitForSeconds(3f);
            // 3. ��� ��� UI�� ��Ȱ��ȭ�Ѵ�.
            ec3.memory.SetActive(false);
            aftermemory = true;
        }
    }

    // �÷��̾��� �ǰ� �Լ�
    public void DamageAction(int damage)
    {

        // �÷��̾��� ������ ���ʹ��� ���ݷº��� Ŭ ��� 1��ŭ�� Ÿ�� 
        if (PlayerStat.instance.DEF >= damage) 
        {
            PlayerStat.instance.currentHP -= 1;
        }
        else // ������ ������ ��ŭ�� �������� ����
        {
            PlayerStat.instance.currentHP -= damage - PlayerStat.instance.DEF;
        }

        // ����, �÷��̾��� ü���� 0���� ũ�� �ǰ� ȿ���� ����Ѵ�.
        if (PlayerStat.instance.currentHP > 0)
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
        // �÷��̾ ���⸦ �����ߴٸ�
        if(PlayerStat.instance.weapon != PlayerStat.instance.emptyItem)
        {
            anim.SetBool("isChange", true);
            PlayerStat.instance.AKT = 5;
        }
        else
        {
            return;
        }
    }
}
