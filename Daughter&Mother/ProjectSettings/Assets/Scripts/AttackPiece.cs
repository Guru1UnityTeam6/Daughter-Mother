using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPiece : MonoBehaviour
{
    // �̵� �ӵ�
    public float speed = 8;
    Vector3 dir;
    //������ ���ݷ�
    public int attackPower = 1;

    //�浹����
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            PlayerController pc = GameObject.Find("Player").GetComponent<PlayerController>();
            PlayerStat.instance.currentHP -= attackPower;
            //�浹���� �� �ް� ����(�ڽ�) �������
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

        //�÷��̾ ã�Ƽ� target���� �ϰ�ʹ�
        GameObject target = GameObject.Find("Player");
        //������ ���ϰ�ʹ�. target - me
        dir = target.transform.position - transform.position;
        //������ ũ�⸦ 1�� �ϰ� �ʹ�.
        dir.Normalize();

        //���� �� �Ѿ��� 3�ʵڿ� ������ �ð������Լ�
        Invoke("DestroyBullet", 3); 
        Debug.Log("�����۵�3"); 
    }

    //�Ѿ��� ������� �Լ�
    void DestroyBullet()
    {
        Destroy(gameObject);
    }

    void Update()
    {
        // 1. ������ ���Ѵ�.
        //Vector3 dir = Vector3.down;
        // 2. �̵��ϰ� �ʹ�. ���� P = P0 + vt
        transform.position += dir * speed * Time.deltaTime;
    }
}