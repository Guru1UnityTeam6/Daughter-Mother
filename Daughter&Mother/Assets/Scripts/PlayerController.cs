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

    // ��ȭâ
    public ChatManager chatManager;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // ���� ����Ű ������ ������ �÷��̾�� �������� �������� ����
        playerMoving = false;

        // ZŰ�� �����ϱ�
        if (Input.GetKey(KeyCode.Z))
        {
            playerAttacking = true;
            anim.SetBool("isAttack(hand)", playerAttacking);
        }
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
                transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));
                playerMoving = true;
                lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
            }

            // ���Ϸ� �����̱�
            if (Input.GetAxisRaw("Vertical") > 0f || Input.GetAxisRaw("Vertical") < 0f)
            {
                transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f));
                playerMoving = true;
                lastMove = new Vector2(0f, Input.GetAxisRaw("Vertical"));
            }
        }
        

        anim.SetFloat("DirX", Input.GetAxisRaw("Horizontal"));
        anim.SetFloat("DirY", Input.GetAxisRaw("Vertical"));
        anim.SetBool("isMove", playerMoving);
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);
    }

    // �÷��̾��� �ǰ� �Լ�
    public void DamageAction(int damage)
    {
        // ���ʹ��� ���ݷ¸�ŭ �÷��̾��� ü���� ��´�.
        hp -= damage;

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
}
